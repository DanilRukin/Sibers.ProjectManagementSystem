using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetRangeOfProjectsQuery : IRequest<IEnumerable<ProjectDto>>
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
