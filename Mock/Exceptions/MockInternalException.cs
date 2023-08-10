using System;

namespace Toubiana.Mock.Exceptions
{
    public class MockInternalException : BaseMockException
    {
        internal MockInternalException(string message)
            : base(message)
        {
        }
    }
}
