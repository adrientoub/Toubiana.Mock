using System;

namespace Toubiana.Mock.Exceptions
{
    public class IncompleteSetupException : Exception
    {
        internal IncompleteSetupException(string methodName)
            : base($"Setup of method '{methodName}' was not completed.")
        {
        }
    }
}
