using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<Result<ProjectDto>>
    {
        public int ProjectId { get; private set; }
        public bool IncludeAdditionalData { get; private set; }

        public GetProjectByIdQuery(int projectId, bool includeAdditionalData)
        {
            ProjectId = projectId;
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
