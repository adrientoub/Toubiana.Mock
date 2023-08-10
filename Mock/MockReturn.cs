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

        internal virtual object GetResult()
        {
            throw new NotImplementedException();
        }
    }

    public class MockReturn<TResult> : MockReturn
    {
        internal MockReturn()
        {
        }

        private TResult? _result = default;
        private bool _isAsync = false;

        public void Returns(TResult result)
        {
            _result = result;
        }

        public void ReturnsAsync(TResult result)
        {
            _result = result;
            _isAsync = true;
        }

        internal override object GetResult()
        {
            if (_isAsync)
            {
                return Task.FromResult(_result);
            }
            return _result;
        }
    }
}
