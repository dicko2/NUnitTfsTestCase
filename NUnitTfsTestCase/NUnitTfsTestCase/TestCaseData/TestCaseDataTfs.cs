using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace NUnitTfsTestCase.TestCaseData
{
    public class TestCaseDataTfs
    {
        /// <summary>
        /// Takes all parameters from specified test case.
        /// Same as <see cref="GetTestDataAsArray"/>, but with casting to dynamic.
        /// Saved to avoid errors while users updating from one version to another.
        /// </summary>
        /// <param name="testCaseId">Number of test case where parameters taken.</param>
        /// <returns>IEnumerable with array of values casted to dynamic.</returns>
        public static IEnumerable<dynamic> GetTestDataInternal(int testCaseId)
        {
            return GetTestDataAsArray(testCaseId);
        }

        /// <summary>
        /// Takes all parameters from specified test case.
        /// Params stored as { ParamValue1, ParamValue2, ParamValue3 }.
        /// Same as <see cref="GetTestDataInternal"/>, but without casting to dynamic. 
        /// </summary>
        /// <param name="testCaseId">Number of test case where parameters taken.</param>
        /// <returns>IEnumerable with array of values. Each object [] represents single line with parameters.</returns>
        public static IEnumerable<object []> GetTestDataAsArray(int testCaseId)
        {
            return GetTestData(testCaseId, CollectTestDataAsArray);
        }

        /// <summary>
        /// Takes all parameters from specified test case.
        /// Params stored as { { ParamName1, ParamValue1 }, { ParamName2, ParamValue2 }, { ParamName3, ParamValue3 } }.
        /// </summary>
        /// <param name="testCaseId">Number of test case where parameters taken.</param>
        /// <returns>IEnumerable with dictionary of names and values. Each Dictionary represents single line with pairs of name and value of parameter.</returns>
        public static IEnumerable<Dictionary<string, object>> GetTestDataAsDictionary(int testCaseId)
        {
            return GetTestData(testCaseId, CollectTestDataAsDictionary);
        }

        private static DataTable GetTestCaseParams(int testCaseId)
        {
            var testCase = TfsService.ProjectConnection.TestManagementProject.TestCases.Find(testCaseId);
            return testCase.DefaultTableReadOnly;
        }

        private static IEnumerable<T> GetTestData<T>(int testCaseId, Func<DataRow, DataColumnCollection, T> storageFunc) where T: IEnumerable
        {
            var tcParams = GetTestCaseParams(testCaseId);
            foreach (DataRow dr in tcParams.Rows)
            {
                yield return storageFunc(dr, tcParams.Columns);
            }
        }

        private static Dictionary<string, object> CollectTestDataAsDictionary(DataRow dr, DataColumnCollection dcc)
        {
            var dict = new Dictionary<string, object>();
            for (int i = 0; i < dcc.Count; i++)
            {
                dict.Add(dcc[i].ColumnName, dr[i]);
            }
            return dict;
        }

        private static object [] CollectTestDataAsArray(DataRow dr, DataColumnCollection dcc)
        {
            var array = new object [dcc.Count];
            for (int i = 0; i < dcc.Count; i++)
            {
                array[i] = dr[i];
            }
            return array;
        }
    }
}
