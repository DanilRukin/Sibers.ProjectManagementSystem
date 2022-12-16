namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos
{
    public class EmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Guid> ExecutableTasksIds { get; set; } = new List<Guid>();
        public List<Guid> CreatedTasksIds { get; set; } = new List<Guid>();
        public int Id { get; set; }
        public List<int> ProjectsIds { get; set; } = new List<int>();
    }
}
