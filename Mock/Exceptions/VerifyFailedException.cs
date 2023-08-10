namespace Toubiana.Mock.Exceptions
{
    public class VerifyFailedException : Exception
    {
        public VerifyFailedException(string methodName, int expectedCount, int actualCount)
            : base($"Verify of {methodName} failed. Expected {expectedCount} calls, got {actualCount}.")
        {
        }
    }
}
