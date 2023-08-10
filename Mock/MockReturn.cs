using System;
using System.Threading.Tasks;
using Toubiana.Mock.Exceptions;

namespace Toubiana.Mock
{
    public class MockReturn
    {
        internal int CallCount { get; private set; } = 0;

        internal MockReturn()
        {
        }

        internal void Call()
        {
            CallCount++;
        }

        internal virtual object? GetResult()
        {
            throw new NotImplementedException();
        }
    }

    public class MockReturn<TResult> : MockReturn
    {
        private readonly string _methodName;

        internal MockReturn(string methodName)
        {
            _methodName = methodName;
        }

        private TResult? _result = default;
        private bool _isAsync = false;
        private bool _isSetup = false;

        public void Returns(TResult result)
        {
            _result = result;
            _isSetup = true;
        }

        public void ReturnsAsync(TResult result)
        {
            Returns(result);
            _isAsync = true;
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
