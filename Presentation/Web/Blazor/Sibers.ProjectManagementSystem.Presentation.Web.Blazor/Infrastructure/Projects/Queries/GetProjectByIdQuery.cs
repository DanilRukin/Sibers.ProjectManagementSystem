using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
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
