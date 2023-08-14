using System.Collections.Generic;

namespace Toubiana.Mock.Exceptions
{
    public class NoMatchingSetupException : BaseMockException
    {
        internal NoMatchingSetupException(string methodName, IList<string> definedMethods)
            : base($"Does not match any setup for {methodName}. Existing setups:\n{string.Join("\n", definedMethods)}")
        {
        }
    }
}
