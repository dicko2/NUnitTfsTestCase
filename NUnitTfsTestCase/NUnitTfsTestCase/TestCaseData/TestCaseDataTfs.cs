using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTfsTestCase.TestCaseData
{
    public class TestCaseDataTfs
    {
        private static DataTable GetTestCaseParams(int TestCaseId)
        {
            int testCaseId = TestCaseId;
            var testCase = TfsService.ProjectConnection.TestManagementProject.TestCases.Find(testCaseId);
            return testCase.DefaultTableReadOnly;
        }

        public static IEnumerable<dynamic> GetTestDataInternal(int TestCaseID)
        {
            var tb = GetTestCaseParams(TestCaseID);
            foreach (DataRow dr in tb.Rows)
            {
                var ob = new List<object>();
                for (var i = 0; i < tb.Columns.Count; i++)
                {
                    ob.Add(dr[i]);
                }
                yield return ob.ToArray();
            }
        }
    }
}
