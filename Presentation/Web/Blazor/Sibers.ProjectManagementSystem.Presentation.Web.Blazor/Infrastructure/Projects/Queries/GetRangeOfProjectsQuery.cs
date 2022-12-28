using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetRangeOfProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public IEnumerable<int> ProjectsIds { get; private set; }
        public bool IncludeAdditionalData { get; private set; }

        public GetRangeOfProjectsQuery(IEnumerable<int> projectsIds, bool includeAdditionalData)
        {
            ProjectsIds = projectsIds ?? throw new ArgumentNullException(nameof(projectsIds));
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
