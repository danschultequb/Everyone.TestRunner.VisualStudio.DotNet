using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Everyone
{
    public class VisualStudioTestRunner : BasicTestRunner
    {
        private readonly bool invokeTests;
        private readonly IEnumerable<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase>? testsToInvoke;

        public VisualStudioTestRunner(
            bool invokeTests = true,
            IEnumerable<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase>? testsToInvoke = null,
            CompareFunctions? compareFunctions = null,
            AssertMessageFunctions? messageFunctions = null,
            ToStringFunctions? toStringFunctions = null)
            : base(compareFunctions, messageFunctions, toStringFunctions)
        {
            this.invokeTests = invokeTests;
            this.testsToInvoke = testsToInvoke;

            this.SetFullNameSeparator(nameof(TestType), ".");
            this.SetFullNameSeparator(nameof(TestMethod), ".");
        }

        public string? CurrentSource { get; set; }

        public Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase TestToTestCase(Test test)
        {
            string fullyQualifiedName = test.GetFullName();

            StackFrame frame = new StackFrame(skipFrames: 6, needFileInfo: true);
            string? codeFilePath = frame.GetFileName();
            int codeFileLineNumber = frame.GetFileLineNumber();

            return new Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase(
                fullyQualifiedName: fullyQualifiedName,
                executorUri: EveryoneTestExecutor.executorUri,
                source: this.CurrentSource ?? string.Empty)
            {
                CodeFilePath = codeFilePath,
                LineNumber = codeFileLineNumber,
            };
        }

        public override bool ShouldInvokeTest(Test test)
        {
            bool result = true;
            if (this.testsToInvoke != null)
            {
                Guid testCaseId = this.TestToTestCase(test).Id;
                result = this.testsToInvoke.Any((Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase testToInvoke) =>
                {
                    return testToInvoke.Id == testCaseId;
                });
            }
            return result;
        }

        public Disposable OnTestStarted(Action<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase> action)
        {
            return this.OnTestStarted((Test test) =>
            {
                action.Invoke(this.TestToTestCase(test));
            });
        }

        public Disposable OnTestPassed(Action<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase> action)
        {
            return this.OnTestPassed((Test test) =>
            {
                action.Invoke(this.TestToTestCase(test));
            });
        }

        public Disposable OnTestFailed(Action<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase, Exception> action)
        {
            return this.OnTestFailed((Test test, Exception exception) =>
            {
                action.Invoke(this.TestToTestCase(test), exception);
            });
        }

        public Disposable OnTestEnded(Action<Microsoft.VisualStudio.TestPlatform.ObjectModel.TestCase> action)
        {
            return this.OnTestEnded((Test test) =>
            {
                action.Invoke(this.TestToTestCase(test));
            });
        }

        protected override void TestInner(TestParameters parameters)
        {
            Pre.Condition.AssertNotNull(parameters, nameof(parameters));
            Pre.Condition.AssertNotNullAndNotEmpty(parameters.GetName(), "parameters.GetName()");
            Pre.Condition.AssertNotNull(parameters.GetAction(), "parameters.GetAction()");

            base.TestInner(parameters
                .SetAction(this.invokeTests ? parameters.GetAction()! : (Test test) => { }));
        }
    }
}
