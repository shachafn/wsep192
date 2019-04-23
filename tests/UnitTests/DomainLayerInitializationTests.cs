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
    public class DomainLayerInitializationTests
    {
        IDomainLayerFacade facade = DomainLayerFacade.Instance;

        [Test]
        public void TestReflection()
        {
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
                catch (Exception ex)
                {
                    // Exceptions thrown inside a MethodBase.Invoke function
                    // are thrown as an inner exception
                    if (!(ex.InnerException is SystemNotInitializedException))
                        Assert.Fail($"method {method.Name} does not throw SystemNotInitializedException");
                }
            }
        }

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
