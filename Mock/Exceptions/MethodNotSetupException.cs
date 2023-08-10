using System;

namespace Toubiana.Mock.Exceptions
{
    public class MethodNotSetupException : BaseMockException
    {
        internal MethodNotSetupException(string methodName)
            : base($"Method {methodName} is not setup")
        {
        }
    }
}
