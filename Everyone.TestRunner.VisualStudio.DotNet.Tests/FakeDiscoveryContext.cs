using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Everyone
{
    public class FakeDiscoveryContext : Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IDiscoveryContext
    {
        public IRunSettings? RunSettings => throw new NotImplementedException();
    }
}
