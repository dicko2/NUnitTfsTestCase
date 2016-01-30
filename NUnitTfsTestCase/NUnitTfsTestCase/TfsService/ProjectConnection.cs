using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using System.Configuration;

namespace NUnitTfsTestCase.TfsService
{
    class ProjectConnection
    {
        private static string tpcUri = ConfigurationManager.AppSettings["tpcUri"];
        private static string teamProjectName = ConfigurationManager.AppSettings["teamProjectName"];
        private static TfsTeamProjectCollection _collection;
        private static ITestManagementTeamProject _testManagement;
        private static ITestManagementService _testManagementService;
        public static TfsTeamProjectCollection Collection
        {
            get
            {
                if (_collection == null) GetTeamProjectCollection();
                return _collection;
            }
        }

        public static ITestManagementTeamProject TestManagementProject
        {
            get
            {
                if (_testManagement == null) GetTeamProjectTestManagementTeamProject();
                return _testManagement;
            }
        }
        public static ITestManagementService TestManagementService
        {
            get
            {
                if (_testManagementService == null) GetTestManagementService();
                return _testManagementService;
            }
        }
        // 
        private static void GetTestManagementService()
        {
            _testManagementService = Collection.GetService<ITestManagementService>();
        }
        private static void GetTeamProjectCollection()
        {
            _collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tpcUri));
            _collection.Connect(Microsoft.TeamFoundation.Framework.Common.ConnectOptions.IncludeServices);
        }

        private static void GetTeamProjectTestManagementTeamProject()
        {
            var service = Collection.GetService<ITestManagementService>();
            _testManagement = service.GetTeamProject(teamProjectName);
        }
    }
}
