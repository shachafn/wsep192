using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DomainLayer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestsUtils;

namespace UnitTests
{
    /// <summary>
    /// Tests that every public function on the facade doesnt get reflection exception when trying to call the verifier function.
    /// </summary>
    [TestFixture]
    public class DomainLayerFacadeVerifierTests
    {
        IDomainLayerFacade facade = MocksCreator.GetDomainLayerFacade();

        [Test]
        public void TestReflection()
        {
            var methods = typeof(IDomainLayerFacade).GetMethods();
            var count = 0;
            var names = new List<string>();
            foreach (var method in methods)
            {
                count++;
                try
                {
                    var parameters = method.GetParameters()
                        .Select(p => GetDefaultValue(p.ParameterType))
                        .ToArray();
                    method.Invoke(facade, parameters);
                }
                catch(VerifierReflectionNotFound)
                {
                    //This is the exception we throw to indicate reflection has failed.
                    Assert.Fail();
                }
                catch(Exception) { } //We dont check actual exceptions, only reflection.
            }
        }

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
