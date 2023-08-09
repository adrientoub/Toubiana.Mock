namespace Toubiana.Mock
{
    public abstract class MockReturn
    {
        // TODO: make it private
        public abstract object GetResult();
    }

    public class MockReturn<TResult> : MockReturn
    {
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

        public override object GetResult()
        {
            if (_isAsync)
            {
                return Task.FromResult(_result);
            }
            return _result;
        }
    }
}
