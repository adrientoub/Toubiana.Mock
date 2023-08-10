using System;

namespace Toubiana.Mock.Exceptions
{
    public class MethodNotSetupException : Exception
    {
        internal MethodNotSetupException(string methodName)
            : base($"Method {methodName} is not setup")
        {
        }
    }
}
