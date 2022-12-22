using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Commands
{
    public class UpdateProjectsDataCommand : IRequest<Result<ProjectDto>>
    {
        public ProjectDto Project { get; private set; }

        public UpdateProjectsDataCommand(ProjectDto project)
        {
            Project = project ?? throw new ArgumentNullException(nameof(project));
        }
    }
}
