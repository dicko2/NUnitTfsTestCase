using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace NUnitTfsTestCase.Actions
{
    public class TfsTestCaseAttribute : Attribute, ITestAction
    {
        private readonly int _tfsTestCaseId;
        private readonly TfsService.TestCases _testCases;
        public TfsTestCaseAttribute(int tfsTestCaseId)
        {
            _tfsTestCaseId = tfsTestCaseId;
            _testCases = new TfsService.TestCases();
        }

        public void BeforeTest(ITest test)
        {
            //            throw new NotImplementedException();
        }

        public void AfterTest(ITest test)
        {
            Console.WriteLine(TestContext.CurrentContext.Result.Outcome.ToString());
            var ErrorMessage = "";
            if (TestContext.CurrentContext.Result.Outcome != ResultState.Success)
            {
                ErrorMessage = TestContext.CurrentContext.Result.Message;
            }
            _testCases.AddTestRun(TfsService.RunData.TestSuiteId, _tfsTestCaseId, TestContext.CurrentContext.Result.Outcome, ErrorMessage,
                TestContext.CurrentContext.Test.Name + " " + TestContext.CurrentContext.Result.Outcome + " " + TestContext.CurrentContext.Result.Message);
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }
    }
}
