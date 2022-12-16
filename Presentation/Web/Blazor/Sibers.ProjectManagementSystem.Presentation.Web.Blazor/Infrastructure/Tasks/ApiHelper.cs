namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Tasks
{
    public static class ApiHelper
    {
        public static string Api { get; set; } = "api/tasks";
        public static class Post
        {
            public static string Create(int projectId, int employeeId)
                => $"{Api}/Create/{projectId}/{employeeId}";
        }

        public static class Put
        {
            public static string Update() => $"{Api}/Update";
            public static string Start(Guid taskId, int projectId, int employeeId)
                => $"{Api}/Start/{taskId}/{projectId}/{employeeId}";
            public static string Complete(Guid taskId, int projectId, int employeeId)
                => $"{Api}/Complete/{taskId}/{projectId}/{employeeId}";
        }
    }
}
