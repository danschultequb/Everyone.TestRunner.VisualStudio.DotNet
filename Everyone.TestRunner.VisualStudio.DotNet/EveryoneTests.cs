using System;
using System.Collections.Generic;
using System.Reflection;

namespace Everyone
{
    public static class EveryoneTests
    {
        public static void InvokeTestMethods(IEnumerable<string> sources, VisualStudioTestRunner runner)
        {
            if (sources == null)
            {
                throw new ArgumentNullException(nameof(sources));
            }
            if (runner == null)
            {
                throw new ArgumentNullException(nameof(runner));
            }

            foreach (string source in sources)
            {
                runner.CurrentSource = source;
                Assembly assembly = Assembly.LoadFrom(source);
                foreach (TypeInfo assemblyType in assembly.DefinedTypes)
                {
                    MethodInfo? testMethodInfo = assemblyType.GetMethod("Test", BindingFlags.Public | BindingFlags.Static, new[] { typeof(TestRunner) });
                    if (testMethodInfo != null)
                    {
                        testMethodInfo.Invoke(obj: null, parameters: new[] { runner });
                    }
                }
            }
            runner.CurrentSource = null;
        }
    }
}
