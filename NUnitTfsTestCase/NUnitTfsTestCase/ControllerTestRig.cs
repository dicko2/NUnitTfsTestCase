using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnitTfsTestCase
{
    class ControllerTestRig
    {
        [OneTimeTearDown]
        public void Stop()
        {
            TfsService.TestCases.FinailizeTestRun();
        }
        [OneTimeSetUp]
        public void Init()
        {
            if (TfsService.RunData.TestSuiteId == 0 || TfsService.RunData.TestSuiteId == 0)
                throw new Exception("Please set both values for test plan and test suite ids");
            TfsService.TestCases.InitializeTestRun(TfsService.RunData.TestPlanId, this.GetType().Name);
        }
    }
}
