using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Toubiana.Mock.Exceptions;

namespace Toubiana.Mock
{
    public class Mock<T>
        where T : class
    {
        private readonly ConcurrentDictionary<string, MockReturn> _setups = new ConcurrentDictionary<string, MockReturn>();

        private readonly Type _interface;

        private const string MockObjectPropertyName = "SuperMock";
        private const string MockTypeName = "MyMockTypeName";

        public Mock()
        {
            if (!typeof(T).IsInterface)
            {
                throw new InvalidOperationException();
            }

            _interface = typeof(T);
        }

        public MockReturn<TResult> Setup<TResult>(Expression<Func<T, TResult>> func)
        {
            var methodName = GetMethodName(func);
            var mockReturn = new MockReturn<TResult>(methodName);
            _setups[methodName] = mockReturn;
            return mockReturn;
        }

        public void Setup(Expression<Action<T>> func)
        {
            var methodName = GetMethodName(func);
            _setups[methodName] = new MockReturn();
        }

        public void Verify<TResult>(Expression<Func<T, TResult>> func, int count)
        {
            var key = GetMethodName(func);
            if (_setups.TryGetValue(key, out var mockReturn))
            {
                if (mockReturn.CallCount != count)
                {
                    throw new VerifyFailedException(key, count, mockReturn.CallCount);
                }
            }
            else
            {
                throw new MethodNotSetupException(key);
            }
        }

        public void Verify(Expression<Action<T>> func, int count)
        {
            var key = GetMethodName(func);
            if (_setups.TryGetValue(key, out var mockReturn))
            {
                if (mockReturn.CallCount != count)
                {
                    throw new VerifyFailedException(key, count, mockReturn.CallCount);
                }
            }
            else
            {
                throw new MethodNotSetupException(key);
            }
        }

        public T Object => BuildObject();

        private static PropertyBuilder DefinePropertyToStoreMock(TypeBuilder typeBuilder)
        {
            var propertyBuilder = typeBuilder.DefineProperty(MockObjectPropertyName, PropertyAttributes.None, typeof(Mock<T>), new Type[0]);

            // Define field
            FieldBuilder fieldBuilder = typeBuilder.DefineField("m_" + MockObjectPropertyName, typeof(Mock<T>), FieldAttributes.Private);
            // Define "getter" for the property
            MethodBuilder getterBuilder = typeBuilder.DefineMethod("get_" + MockObjectPropertyName,
                                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                typeof(Mock<T>),
                                                Type.EmptyTypes);
            ILGenerator getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);

            // Define "setter" for the property
            MethodBuilder setterBuilder = typeBuilder.DefineMethod("set_" + MockObjectPropertyName,
                                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                null,
                                                new Type[] { typeof(Mock<T>) });
            ILGenerator setterIL = setterBuilder.GetILGenerator();
            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fieldBuilder);
            setterIL.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterBuilder);
            propertyBuilder.SetSetMethod(setterBuilder);
            return propertyBuilder;
        }

        internal object? GetMethodReturnValue(string methodName)
        {
            if (_setups.TryGetValue(methodName, out var methodResult))
            {
                methodResult.Call();
                return methodResult.GetResult();
            }

            throw new MethodNotSetupException(methodName);
        }

        // Called by the mocked object to validate that the method was setup when the method returns void.
        internal void ValidateMethodSetup(string methodName)
        {
            if (!_setups.TryGetValue(methodName, out var mockReturn))
            {
                throw new MethodNotSetupException(methodName);
            }

            mockReturn.Call();
        }

        private T BuildObject()
        {
            var dynamicClassDefinition = CreateDynamicType(_interface);
            T? instance = (T?)Activator.CreateInstance(dynamicClassDefinition);
            if (instance == null)
            {
                throw new MockInternalException($"Could not setup proxy type for base class {_interface.FullName}");
            }

            // Store a reference to the mock inside the object.
            var property = instance.GetType().GetProperty(MockObjectPropertyName);
            if (property == null)
            {
                throw new MockInternalException($"Could not find internal property in proxy class.");
            }

            property.SetValue(instance, this);

            return instance;
        }

        private static string GetMethodName<TDelegate>(Expression<TDelegate> func)
        {
            if (func.Body is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }
            else if (func.Body is MemberExpression memberExpression)
            {
                return "get_" + memberExpression.Member.Name;
            }

            throw new ExpressionTooComplexException();
        }

        /// <summary>
        /// Creates a dynamic type implementing the mocked interface.
        /// </summary>
        private static Type CreateDynamicType(Type interfaceToImplement)
        {
            AssemblyName assemblyName = new AssemblyName(Constants.MockAssemblyName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            TypeBuilder typeBuilder = moduleBuilder.DefineType(MockTypeName, TypeAttributes.Public);

            typeBuilder.AddInterfaceImplementation(interfaceToImplement);
            PropertyBuilder propertyBuilder = DefinePropertyToStoreMock(typeBuilder);

            foreach (var method in interfaceToImplement.GetMethods())
            {
                DefineProxyMethod(typeBuilder, propertyBuilder, method);
            }

            // Create the type.
            Type? dynamicType = typeBuilder.CreateType();
            if (dynamicType == null)
            {
                throw new MockInternalException($"Could not create dynamic type for base class {interfaceToImplement.FullName}");
            }

            return dynamicType;
        }

        /// <summary>
        /// Define a proxy method for the given interface method.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="property">The property where the mock is stored.</param>
        /// <param name="method">The interface method definition that we need to define a proxy method for.</param>
        private static void DefineProxyMethod(TypeBuilder typeBuilder, PropertyInfo property, MethodInfo method)
        {
            MethodInfo? getMockReturnMethod = property.GetGetMethod();
            if (getMockReturnMethod == null)
            {
                throw new MockInternalException($"Could not find internal property getter in proxy class.");
            }
            MethodInfo? methodToCall = ExtractMethodToCallToRetrieveValue(method);
            if (methodToCall == null)
            {
                throw new MockInternalException($"Could not find method to call to retrieve value.");
            }

            var newAttributes = method.Attributes;
            newAttributes &= ~MethodAttributes.Abstract;
            newAttributes &= ~MethodAttributes.NewSlot;

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, newAttributes, method.ReturnType, method.GetParameters().Select(x => x.ParameterType).ToArray());
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Callvirt, getMockReturnMethod);
            ilGenerator.Emit(OpCodes.Ldstr, method.Name);
            ilGenerator.Emit(OpCodes.Callvirt, methodToCall);

            // Unbox structs.
            if (method.ReturnType != typeof(void) && method.ReturnType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, method.ReturnType);
            }
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static MethodInfo? ExtractMethodToCallToRetrieveValue(MethodInfo method)
        {
            string methodName;
            if (method.ReturnType == typeof(void))
            {
                methodName = nameof(ValidateMethodSetup);
            }
            else
            {
                methodName = nameof(GetMethodReturnValue);
            }

#if NET5_0_OR_GREATER
            return typeof(Mock<T>).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, new Type[] { typeof(string), });
#else
            return typeof(Mock<T>).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(string), }, null);
#endif
        }
    }
}
