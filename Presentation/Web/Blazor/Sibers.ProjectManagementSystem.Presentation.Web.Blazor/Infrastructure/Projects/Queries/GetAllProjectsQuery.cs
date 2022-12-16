using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
        public bool IncludeAdditionalData { get; private set; }

        public GetAllProjectsQuery(bool includeAdditionalData)
        {
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
