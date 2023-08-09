using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Toubiana.Mock
{
    public class Mock<T>
        where T : class
    {
        private readonly ConcurrentDictionary<string, MockReturn> _setups = new ConcurrentDictionary<string, MockReturn>();

        private readonly Type _type;

        private const string MockObjectPropertyName = "SuperMock";
        private const string MockTypeName = "MyMockTypeName";

        public Mock()
        {
            if (!typeof(T).IsInterface)
            {
                throw new InvalidOperationException();
            }

            _type = typeof(T);
        }

        public static string GetMethodName<TResult>(Expression<Func<T, TResult>> func)
        {
            if (func.Body is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }

            return null;
        }

        public MockReturn<TResult> Setup<TResult>(Expression<Func<T, TResult>> func)
        {
            var key = GetMethodName(func);
            var mockReturn = new MockReturn<TResult>();
            _setups[key] = mockReturn;
            return mockReturn;
        }

        public T Object => BuildObject();

        private static PropertyBuilder DefinePropertyToStoreMock(TypeBuilder typeBuilder)
        {
            var propertyBuilder = typeBuilder.DefineProperty(MockObjectPropertyName, PropertyAttributes.None, typeof(Mock<T>), new Type[0]);

            // Define field
            FieldBuilder fieldBuilder = typeBuilder.DefineField("m_" + MockObjectPropertyName, typeof(Mock<T>), FieldAttributes.Private);
            // Define "getter" for MyChild property
            MethodBuilder getterBuilder = typeBuilder.DefineMethod("get_" + MockObjectPropertyName,
                                                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                                                typeof(Mock<T>),
                                                Type.EmptyTypes);
            ILGenerator getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIL.Emit(OpCodes.Ret);

            // Define "setter" for MyChild property
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

        public object GetReturn(string methodName)
        {
            if (_setups.TryGetValue(methodName, out var methodResult))
            {
                return methodResult.GetResult();
            }

            throw new Exception();
        }

        private T BuildObject()
        {
            var dynamicClassDefinition = CreateDynamicType();
            T instance = (T)Activator.CreateInstance(dynamicClassDefinition);

            // Store a reference to the mock inside the object.
            var property = instance.GetType().GetProperty(MockObjectPropertyName);
            property.SetValue(instance, this);

            return instance;
        }

        /// <summary>
        /// Creates a dynamic type implementing the mocked interface.
        /// </summary>
        private Type CreateDynamicType()
        {
            AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            // TODO: update the name of the type
            TypeBuilder typeBuilder = moduleBuilder.DefineType(MockTypeName, TypeAttributes.Public);

            typeBuilder.AddInterfaceImplementation(_type);
            PropertyBuilder propertyBuilder = DefinePropertyToStoreMock(typeBuilder);

            foreach (var method in _type.GetMethods())
            {
                var newAttributes = method.Attributes;
                newAttributes &= ~MethodAttributes.Abstract;
                newAttributes &= ~MethodAttributes.NewSlot;

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name, newAttributes, method.ReturnType, method.GetParameters().Select(x => x.ParameterType).ToArray());
                ILGenerator ilGenerator = methodBuilder.GetILGenerator();

                MethodInfo getReturnValue = this.GetType().GetMethod(nameof(GetReturn), BindingFlags.Instance | BindingFlags.Public, new Type[] { typeof(string), });

                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Callvirt, propertyBuilder.GetGetMethod());
                ilGenerator.Emit(OpCodes.Ldstr, method.Name);
                ilGenerator.Emit(OpCodes.Callvirt, getReturnValue);
                ilGenerator.Emit(OpCodes.Ret);
            }

            // Create the type.
            Type dynamicType = typeBuilder.CreateType();

            return dynamicType;
        }
    }
}
