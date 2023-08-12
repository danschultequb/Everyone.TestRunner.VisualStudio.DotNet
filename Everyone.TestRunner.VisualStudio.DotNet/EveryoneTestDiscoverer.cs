using System.Collections.Generic;

namespace Everyone
{
    [Microsoft.VisualStudio.TestPlatform.ObjectModel.FileExtension(".exe")]
    [Microsoft.VisualStudio.TestPlatform.ObjectModel.FileExtension(".dll")]
    [Microsoft.VisualStudio.TestPlatform.ObjectModel.DefaultExecutorUri(EveryoneTestExecutor.executorUriString)]
    [System.ComponentModel.Category("managed")]
    public class EveryoneTestDiscoverer : Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestDiscoverer
    {
        public void DiscoverTests(
            IEnumerable<string> sources,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IDiscoveryContext discoveryContext,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging.IMessageLogger logger,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestCaseDiscoverySink discoverySink)
        {
            PreCondition.AssertNotNull(discoveryContext, nameof(discoveryContext));

            VisualStudioTestRunner runner = new VisualStudioTestRunner(invokeTests: false);
            runner.OnTestStarted((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testCase) =>
            {
                discoverySink.SendTestCase(testCase);
            });
            EveryoneTests.InvokeTestMethods(sources, runner);
        }
    }
}