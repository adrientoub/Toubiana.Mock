using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Toubiana.Mock.Exceptions;

namespace Toubiana.Mock
{
    internal class MultiSetupMethodReturn
    {
        private readonly ConcurrentBag<MockReturn> _setups = new ConcurrentBag<MockReturn>();
        private readonly string _methodName;

        // TODO: store all the calls here instead of inside the MockReturn object

        public MultiSetupMethodReturn(string methodName)
        {
            _methodName = methodName;
        }

        internal void AddSetup(MockReturn mockReturn)
        {
            _setups.Add(mockReturn);
        }

        internal MockReturn GetSetup(IList<object?> actualArguments)
        {
            foreach (var setup in _setups)
            {
                if (setup.DoesMatch(actualArguments))
                {
                    return setup;
                }
            }

            throw new NoMatchingSetupException(_methodName, _setups.Select(s => s.MethodDefinitionToString()).ToList());
        }
    }
}
