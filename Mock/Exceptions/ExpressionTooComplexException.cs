using System;

namespace Toubiana.Mock.Exceptions
{
    public class ExpressionTooComplexException : Exception
    {
        internal ExpressionTooComplexException()
            : base("The Mock only works with simple expressions.")
        {
        }
    }
}
