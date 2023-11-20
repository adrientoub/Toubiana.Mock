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

        protected Exception? _exception;
        protected bool _isSetup = false;
        protected bool _isVerifiable = false;

        internal int CallCount { get; private set; } = 0;

        internal MockReturn(string methodName, List<ItMatcher> argumentMatchers)
            : this(methodName, argumentMatchers, true)
        {
        }

        private protected MockReturn(string methodName, List<ItMatcher> argumentMatchers, bool isSetup)
        {
            _methodName = methodName;
            _argumentMatchers = argumentMatchers;
            _isSetup = isSetup;
        }

        public void Throws(Exception exception)
        {
            _exception = exception;
            _isSetup = true;
        }

        private void RegisterCall()
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
            if (!_isSetup)
            {
                throw new IncompleteSetupException(_methodName);
            }
            this.RegisterCall();

            if (_exception != null)
            {
                throw _exception;
            }

            return null;
        }

        public void Verifiable()
        {
            _isVerifiable = true;
        }

        /// <summary>
        /// Checks that verifiable setup has been called at least once.
        /// </summary>
        internal void Verify()
        {
            if (_isVerifiable && CallCount == 0)
            {
                throw new VerifyFailedException(MethodDefinitionToString(), Times.AtLeastOnce(), 0);
            }
        }
    }

    public class MockReturn<TResult> : MockReturn
    {
        internal MockReturn(string methodName, List<ItMatcher> argumentMatchers)
            : base(methodName, argumentMatchers, false)
        {
        }

        private TResult? _result = default;
        private Delegate? _resultDelegate = default;

        public void Returns(TResult result)
        {
            _resultDelegate = null;
            _exception = null;
            _result = result;
            _isSetup = true;
        }

        public void Returns<T>(Func<TResult> result)
        {
            _resultDelegate = result;
            _exception = null;
            _result = default;
            _isSetup = true;
        }

        internal override object? GetResult()
        {
            base.GetResult();

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
