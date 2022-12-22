namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects
{
    public static class ApiHelper
    {
        public static string _api = "api/projects";

        public static class Get
        {
            public static string ById(int id, bool includeAdditionalData = false)
                => $"{_api}/{id}/{includeAdditionalData}";
            public static string All(bool includeAdditionalData = false)
                => $"{_api}/all?includeAdditionalData={includeAdditionalData}";
            public static string Range(bool includeAdditionalData = false)
                => $"{_api}/range/{includeAdditionalData}";
        }

        public static class Put
        {
            public static string AddEmployee(int projectId, int employeeId)
                => $"{_api}/addemployee/{projectId}/{employeeId}";
            public static string RemoveEmployee(int projectId, int emplyeeId)
                => $"{_api}/removeemployee/{projectId}/{emplyeeId}";
            public static string FireManager(int projectId, string reason)
                => $"{_api}/firemanager/{projectId}/{reason}";
            public static string DemoteManager(int projectId, string reason)
                => $"{_api}/demotemanager/{projectId}/{reason}";
            public static string PromoteEmployeeToManager(int projectId, int employeeId)
                => $"{_api}/promoteemployee/{projectId}/{employeeId}";
            public static string TransferEmployee(int currentProjectId, int futureProjectId, int employeeId)
                => $"{_api}/transferemployee/{currentProjectId}/{futureProjectId}/{employeeId}";
            public static string AddRangeOfEmployees(int projectId)
                => $"{_api}/addrangeofemployees{projectId}";
            public static string Update() => $"{_api}/update";
        }

        public static class Post
        {
            public static string Create() => _api;
        }

        public static class Delete
        {
            public static string ById(int id) => $"{_api}/{id}";
        }
    }
}
