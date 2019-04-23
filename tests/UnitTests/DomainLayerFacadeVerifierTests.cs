using DomainLayer.Exceptions;
using DomainLayer.Facade;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnitTests
{
    /// <summary>
    /// Tests that every public function on the facade doesnt get reflection exception when trying to call the verifier function.
    /// </summary>
    [TestFixture]
    public class DomainLayerFacadeVerifierTests
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;

        [Test]
        public void TestReflection()
        {
            TestInitializeReflection();
            
            var methods = typeof(IDomainLayerFacade).GetMethods();
            foreach (var method in methods)
            {
                var name = method.Name;
                if (name.Equals("Initialize"))
                    continue;

                try
                {
                    var parameters = method.GetParameters()
                        .Select(p => GetDefaultValue(p.ParameterType))
                        .ToArray();
                    method.Invoke(facade, parameters);
                }
                catch(Exception ex)
                {
                    // Exceptions thrown inside a MethodBase.Invoke function
                    // are thrown as an inner exception
                    // This is the exception we throw to indicate reflection has failed.
                    if (ex.InnerException is VerifierReflectionNotFound)
                        Assert.Fail($"Problem Verifying method {method.Name}");
                    //We dont check actual exceptions, only reflection.
                }
            }
        }

        private void TestInitializeReflection()
        {
            try
            {
                facade.Initialize(new DomainLayer.ExposedClasses.UserIdentifier(Guid.NewGuid(), true), "admin", "0000");
            }
            catch(VerifierReflectionNotFound)
            {
                Assert.Fail($"Problem Verifying Initialize");

            }
            catch (Exception) { }
        }

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
