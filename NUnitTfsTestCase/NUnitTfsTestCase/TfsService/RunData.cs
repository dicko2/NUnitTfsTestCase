using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.TestManagement.Client;

namespace NUnitTfsTestCase.TfsService
{
    public class RunData
    {
        public static ITestRun Testrun;
        public static ITestPlan Plan;
        public static List<RunCache> RunCacheData;
        public static int TestSuiteId;
        public static int TestPlanId;
    }
}
