using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using NUnit.Framework.Interfaces;

namespace NUnitTfsTestCase.TfsService
{
    public class TestCases
    {
        public static void InitializeTestRun(int TestplanId, string planTitle)
        {
            //Get user name
            TeamFoundationIdentity tfi = TfsService.ProjectConnection.TestManagementService.AuthorizedIdentity;

            //Get test plan
            RunData.Plan = TfsService.ProjectConnection.TestManagementProject.TestPlans.Find(TestplanId);
            //Environment.GetEnvironmentVariable().
            //env:TfsBuildName
            //env:TfsBuildNumber
            //Create test run
            RunData.Testrun = RunData.Plan.CreateTestRun(false);
            RunData.Testrun.Owner = tfi;
            RunData.Testrun.Title = "Unit Test Run for " + planTitle;

            RunData.RunCacheData = new List<RunCache>();
        }
        public void AddTestRun(int suiteId, int testCaseId, ResultState caseResult, string errorOuput, string comment)
        {
            if (RunData.Testrun == null) throw new Exception("InitializeTestRun method has not been called, please call this at the start of the test run");

            //Get test points
            ITestPointCollection testpoints = RunData.Plan.QueryTestPoints("SELECT * FROM TestPoint WHERE SuiteId = " + suiteId + " and TestCaseId = " + testCaseId);

            foreach (ITestPoint tp in testpoints)
            {
                RunData.Testrun.AddTestPoint(tp, null);
            }
            RunData.RunCacheData.Add(new RunCache() { CaseResult = caseResult, Comment = comment, ErrorOutput = errorOuput });
        }

        public static void FinailizeTestRun()
        {
            var x = 0;
            //Create test result
            RunData.Testrun.Save();

            ITestCaseResultCollection results = RunData.Testrun.QueryResults();

            foreach (ITestCaseResult testresult in results)
            {
                var i = RunData.RunCacheData[x];
                x++;// there is an assumption that we will have one object per test in the run 
                testresult.RunBy = RunData.Testrun.Owner;
                testresult.Outcome = TranslateTestResult(i.CaseResult);
                testresult.State = TestResultState.Completed;
                testresult.ErrorMessage = i.ErrorOutput;
                testresult.Comment = i.Comment;
                testresult.Save();
            }
        }

        private static TestOutcome TranslateTestResult(ResultState result)
        {
            if (result == ResultState.Success) return TestOutcome.Passed;
            if (result == ResultState.Cancelled) return TestOutcome.Aborted;
            if (result == ResultState.ChildFailure) return TestOutcome.Blocked;
            if (result == ResultState.Error) return TestOutcome.Error;
            if (result == ResultState.Explicit) return TestOutcome.Failed;
            if (result == ResultState.Failure) return TestOutcome.Failed;
            if (result == ResultState.Ignored) return TestOutcome.NotExecuted;
            if (result == ResultState.Inconclusive) return TestOutcome.Inconclusive;
            if (result == ResultState.NotRunnable) return TestOutcome.NotExecuted;
            if (result == ResultState.SetUpError) return TestOutcome.Blocked;
            if (result == ResultState.SetUpFailure) return TestOutcome.Blocked;
            if (result == ResultState.Cancelled) return TestOutcome.Aborted;
            if (result == ResultState.Skipped) return TestOutcome.NotExecuted;
            if (result == ResultState.TearDownError) return TestOutcome.Unspecified;
            throw new Exception(String.Format("Could not translate result from NUnit result was {0}", result));
        }
    }
}
