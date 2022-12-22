using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Queries.ProjectQueries
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
