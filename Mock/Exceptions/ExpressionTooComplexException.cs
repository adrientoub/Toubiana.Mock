using System;

namespace Toubiana.Mock.Exceptions
{
    public class ExpressionTooComplexException : BaseMockException
    {
        internal ExpressionTooComplexException()
            : base("The Mock only works with simple expressions.")
        {
        }
    }
}
