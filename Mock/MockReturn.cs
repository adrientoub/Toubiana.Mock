using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Toubiana.Mock.Exceptions;

namespace Toubiana.Mock
{
    public class MockReturn
    {
        protected readonly string _methodName;
        private readonly List<ItMatcher> _argumentMatchers;

        internal int CallCount { get; private set; } = 0;

        internal MockReturn(string methodName, List<ItMatcher> argumentMatchers)
        {
            _methodName = methodName;
            _argumentMatchers = argumentMatchers;
        }

        internal void Call()
        {
            CallCount++;
        }

        internal bool DoesMatch(IList<object?> actualArguments)
        {
            if (actualArguments.Count != _argumentMatchers.Count)
            {
                return false;
            }

            for (int i = 0; i < actualArguments.Count; i++)
            {
                if (!_argumentMatchers[i].IsMatch(actualArguments[i]))
                {
                    return false;
                }
            }

            return true;
        }

        internal string MethodDefinitionToString()
        {
            return $"{_methodName}({string.Join(", ", _argumentMatchers)})";
        }

        internal virtual object? GetResult()
        {
            throw new NotImplementedException();
        }
    }

    public class MockReturn<TResult> : MockReturn
    {
        internal MockReturn(string methodName, List<ItMatcher> argumentMatchers)
            : base(methodName, argumentMatchers)
        {
        }

        private TResult? _result = default;
        private Delegate? _resultDelegate = default;
        private bool _isSetup = false;

        public void Returns(TResult result)
        {
            _resultDelegate = null;
            _result = result;
            _isSetup = true;
        }

        public void Returns<T>(Func<TResult> result)
        {
            _resultDelegate = result;
            _result = default;
            _isSetup = true;
        }

        internal override object? GetResult()
        {
            if (!_isSetup)
            {
                throw new IncompleteSetupException(_methodName);
            }

            if (_resultDelegate != null)
            {
                return _resultDelegate.DynamicInvoke();
            }

            return _result;
        }
    }

    public class MockAsyncReturn<TResult> : MockReturn<Task<TResult>>
    {
        internal MockAsyncReturn(string methodName, List<ItMatcher> argumentMatchers)
            : base(methodName, argumentMatchers)
        {
        }

        public void ReturnsAsync(Func<TResult> func)
        {
            Returns<Task<TResult>>(() => Task.FromResult(func()));
        }

        public void ReturnsAsync(TResult value)
        {
            Returns(Task.FromResult(value));
        }
    }
}
