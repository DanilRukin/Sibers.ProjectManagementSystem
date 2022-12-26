namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees
{
    public static class ApiHelper
    {
        public static string Api { get; set; } = "api/employees";
        public static class Get
        {
            public static string ById(int employeeId, bool includeData = false)
                => $"{Api}/{employeeId}/{includeData}";
            public static string All(bool includeAdditionalData = false)
                => $"{Api}/all/{includeAdditionalData}";
            public static string Range(bool includeAdditionalData = false)
                => $"{Api}/range/{includeAdditionalData}";
        }

        public static class Post
        {
            public static string Create() => $"{Api}";
            public static string CreateTask(int projectId, int employeeId)
                => $"{Api}/CreateTask/{projectId}/{employeeId}";
        }

        public static class Put
        {
            public static string StartTask(Guid taskId, int employeeId, int projectId)
                => $"{Api}/StartTask/{taskId}/{projectId}/{employeeId}";
            public static string CompleteTask(Guid taskId, int employeeId, int projectId)
                => $"{Api}/CompleteTask/{taskId}/{projectId}/{employeeId}";
            public static string Update() => $"{Api}/update";
        }

        public static class Delete
        {
            public static string ById(int id) => $"{Api}/{id}";
        }
    }
}
