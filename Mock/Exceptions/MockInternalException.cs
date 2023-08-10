using System;

namespace Toubiana.Mock.Exceptions
{
    public class MockInternalException : Exception
    {
        internal MockInternalException(string message)
            : base(message)
        {
        }
    }
}
