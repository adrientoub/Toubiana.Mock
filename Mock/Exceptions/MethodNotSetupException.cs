namespace Toubiana.Mock.Exceptions
{
    public class MethodNotSetupException : Exception
    {
        public MethodNotSetupException(string methodName)
            : base($"Method {methodName} is not setup")
        {
        }
    }
}
