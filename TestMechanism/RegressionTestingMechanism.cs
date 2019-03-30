using System;
using System.Collections.Generic;
using System.Text;

namespace TestingMechanism
{
    public static class RegressionTestingMechanism
    {
        private static List<ITest> _tests = new List<ITest>();

        public static void RunTests()
        {
            _tests.ForEach(test => test.Run());
        }
        public static void AddTest(ITest test)
        {
            if (!_tests.Contains(test))
                _tests.Add(test);
        }

        public static void RemoveTest(ITest test)
        {
            if (_tests.Contains(test))
                _tests.Remove(test);
        }
    }
}
