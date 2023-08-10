using System;

namespace Toubiana.Mock.Exceptions
{
    public abstract class BaseMockException : Exception
    {
        internal BaseMockException(string message) : base(message)
        {
        }
    }
}
