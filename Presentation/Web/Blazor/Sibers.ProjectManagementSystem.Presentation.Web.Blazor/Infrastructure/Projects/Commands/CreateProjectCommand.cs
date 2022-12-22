using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Commands
{
    public class CreateProjectCommand : IRequest<Result<ProjectDto>>
    {
        public ProjectDto ProjectDto { get; private set; }

        public CreateProjectCommand(ProjectDto projectDto)
        {
            ProjectDto = projectDto ?? throw new ArgumentNullException(nameof(projectDto));
        }
    }
}
