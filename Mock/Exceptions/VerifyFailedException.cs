namespace Toubiana.Mock.Exceptions
{
    public class VerifyFailedException : BaseMockException
    {
        internal VerifyFailedException(string methodName, Times times, int actualCount)
            : base($"Verify of {methodName} failed. Expected {times} calls, got {actualCount}.")
        {
        }
    }
}
