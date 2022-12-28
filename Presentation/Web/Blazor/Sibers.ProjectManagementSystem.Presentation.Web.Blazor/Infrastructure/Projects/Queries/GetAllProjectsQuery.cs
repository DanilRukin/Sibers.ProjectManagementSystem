using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public bool IncludeAdditionalData { get; private set; }

        public GetAllProjectsQuery(bool includeAdditionalData)
        {
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
