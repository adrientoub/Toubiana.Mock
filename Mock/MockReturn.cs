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
        private bool _isAsync = false;
        private bool _isSetup = false;

        public void Returns(TResult result)
        {
            _result = result;
            _isSetup = true;
        }

        internal override object? GetResult()
        {
            if (!_isSetup)
            {
                throw new IncompleteSetupException(_methodName);
            }

            if (_isAsync)
            {
                return Task.FromResult(_result);
            }
            return _result;
        }
    }
}
