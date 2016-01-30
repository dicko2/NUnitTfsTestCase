using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;

namespace NUnitTfsTestCase.TestCaseData
{
    public class TestCaseDataCsvReader
    {
        public string MyCsvFile { get; set; }
        public List<string> ColumnNames { get; set; }
        public IEnumerable<dynamic> GetTestData()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = MyCsvFile;
            int x = 0;
            var d = new List<string>();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var csv = new CsvReader(new StreamReader(stream), true, '\t'))
            {
                while (csv.ReadNextRecord())
                {
                    var ob = new List<string>();
                    for (var i = 0; i < csv.Columns.Count; i++)
                    {
                        ob.Add(csv[i]);
                    }
                    yield return ob.ToArray();

                }
            }
        }
    }
}
