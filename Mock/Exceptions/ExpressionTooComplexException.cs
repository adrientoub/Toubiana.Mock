namespace Toubiana.Mock.Exceptions
{
    public class ExpressionTooComplexException : Exception
    {
        public ExpressionTooComplexException()
            : base("The Mock only works with simple expressions.")
        {
        }
    }
}
