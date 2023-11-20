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

        internal MockReturn? GetSetup(IList<object?> actualArguments, bool nullIfNotFound)
        {
            foreach (var setup in _setups)
            {
                if (setup.DoesMatch(actualArguments))
                {
                    return setup;
                }
            }

            if (nullIfNotFound)
            {
                return null;
            }

            throw new NoMatchingSetupException(_methodName, _setups.Select(s => s.MethodDefinitionToString()).ToList());
        }

        internal void VerifyAll()
        {
            foreach (var setup in _setups)
            {
                if (setup.CallCount == 0)
                {
                    throw new VerifyFailedException(setup.MethodDefinitionToString(), Times.AtLeastOnce(), 0);
                }
            }
        }

        /// <summary>
        /// Verify that all the verifiable setups have been called at least once.
        /// </summary>
        internal void Verify()
        {
            foreach (var setup in _setups)
            {
                setup.Verify();
            }
        }
    }
}
