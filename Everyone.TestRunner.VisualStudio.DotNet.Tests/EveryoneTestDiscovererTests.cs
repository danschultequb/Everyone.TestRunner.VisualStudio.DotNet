using System;
using System.Collections.Generic;

namespace Everyone
{
    public static class EveryoneTestDiscovererTests
    {
        public static void Test(TestRunner runner)
        {
            runner.TestType<EveryoneTestDiscoverer>(() =>
            {
                runner.TestMethod("Constructor()", (Test test) =>
                {
                    EveryoneTestDiscoverer discoverer = new EveryoneTestDiscoverer();
                    test.AssertNotNull(discoverer);
                });

                runner.TestMethod("DiscoverTests()", () =>
                {
                    runner.Test("with null arguments", (Test test) =>
                    {
                        EveryoneTestDiscoverer discoverer = new EveryoneTestDiscoverer();
                        test.AssertThrows(() => discoverer.DiscoverTests(sources: null!, discoveryContext: null!, logger: null!, discoverySink: null!),
                            new PreConditionFailure(
                                "Expression: discoveryContext",
                                "Expected: not null",
                                "Actual:       null"));
                    });

                    runner.Test("with empty sources", (Test test) =>
                    {
                        EveryoneTestDiscoverer discoverer = new EveryoneTestDiscoverer();
                        IEnumerable<string> sources = new string[0];
                        FakeDiscoveryContext discoveryContext = new FakeDiscoveryContext();
                        FakeMessageLogger logger = new FakeMessageLogger();
                        FakeTestCaseDiscoverySink discoverySink = new FakeTestCaseDiscoverySink();

                        discoverer.DiscoverTests(sources, discoveryContext, logger, discoverySink);
                    });

                    //runner.Test("with one source", (Test test) =>
                    //{
                    //    EveryoneTestDiscoverer discoverer = new EveryoneTestDiscoverer();
                    //    IEnumerable<string> sources = new[] { "fake-source" };
                    //    FakeDiscoveryContext discoveryContext = new FakeDiscoveryContext();
                    //    FakeMessageLogger logger = new FakeMessageLogger();
                    //    FakeTestCaseDiscoverySink discoverySink = new FakeTestCaseDiscoverySink();

                    //    discoverer.DiscoverTests(sources, discoveryContext, logger, discoverySink);
                    //});
                });
            });
        }
    }
}
