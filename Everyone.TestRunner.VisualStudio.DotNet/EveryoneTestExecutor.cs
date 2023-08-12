using System;
using System.Collections.Generic;
using System.Linq;

namespace Everyone
{
    [Microsoft.VisualStudio.TestPlatform.ObjectModel.ExtensionUri(EveryoneTestExecutor.executorUriString)]
    public class EveryoneTestExecutor : Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.ITestExecutor
    {
        public const string executorUriString = "executor://everyone.everyonetestadapter/";
        public static readonly Uri executorUri = new Uri(EveryoneTestExecutor.executorUriString);

        public void RunTests(
            IEnumerable<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase>? tests,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IRunContext? runContext,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IFrameworkHandle? frameworkHandle)
        {
            if (tests?.Any() == true && frameworkHandle != null)
            {
                VisualStudioTestRunner runner = EveryoneTestExecutor.CreateTestRunner(frameworkHandle);
                EveryoneTests.InvokeTestMethods(
                    sources: tests.Select(test => test.Source).Distinct(),
                    runner: runner);
            }
        }

        public void RunTests(
            IEnumerable<string>? sources,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IRunContext? runContext,
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IFrameworkHandle? frameworkHandle)
        {
            if (sources?.Any() == true && frameworkHandle != null)
            {
                VisualStudioTestRunner runner = EveryoneTestExecutor.CreateTestRunner(frameworkHandle);
                EveryoneTests.InvokeTestMethods(sources, runner);
            }
        }

        private static VisualStudioTestRunner CreateTestRunner(
            Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter.IFrameworkHandle frameworkHandle)
        {
            VisualStudioTestRunner runner = new VisualStudioTestRunner();

            var testCaseResults = new Dictionary<Guid, Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult>();
            runner.OnTestStarted((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testCase) =>
            {
                var testResult = new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult(testCase);
                testCaseResults.Add(testCase.Id, testResult);

                frameworkHandle.RecordStart(testCase);
            });
            runner.OnTestPassed((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testCase) =>
            {
                Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult testResult = testCaseResults[testCase.Id];
                testResult.Outcome = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.Passed;
            });
            runner.OnTestFailed((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testCase, Exception exception) =>
            {
                Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult testResult = testCaseResults[testCase.Id];
                testResult.ErrorMessage = exception.Message;
                testResult.ErrorStackTrace = exception.StackTrace;
                testResult.Outcome = Microsoft.VisualStudio.TestPlatform.ObjectModel.TestOutcome.Failed;
            });
            runner.OnTestEnded((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testCase) =>
            {
                Microsoft.VisualStudio.TestPlatform.ObjectModel.TestResult testResult = testCaseResults[testCase.Id];
                testResult.EndTime = DateTimeOffset.Now;
                testResult.Duration = testResult.EndTime.Subtract(testResult.StartTime);
                testCaseResults.Remove(testCase.Id);

                frameworkHandle.RecordResult(testResult);
                frameworkHandle.RecordEnd(testCase, testResult.Outcome);
            });


            return runner;
        }

        public void Cancel()
        {
        }
    }
}