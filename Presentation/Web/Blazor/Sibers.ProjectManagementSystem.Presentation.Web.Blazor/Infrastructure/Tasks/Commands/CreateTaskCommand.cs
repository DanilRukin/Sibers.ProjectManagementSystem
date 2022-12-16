using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public int ProjectId { get; private set; }
        public int EmployeeId { get; private set; }
        public TaskDto Task { get; private set; }

        public CreateTaskCommand(int projectId, int employeeId, TaskDto task)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }
    }
}
