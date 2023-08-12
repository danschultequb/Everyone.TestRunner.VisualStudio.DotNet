using System;

namespace Everyone
{
    internal class FakeTestCaseDiscoverySink : Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestCaseDiscoverySink
    {
        public void SendTestCase(Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase discoveredTest)
        {
            throw new NotImplementedException();
        }
    }
}
