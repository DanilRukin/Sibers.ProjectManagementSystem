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
    public class GetAllProjectsQuery : IRequest<Result<IEnumerable<ProjectDto>>>
    {
        public bool IncludeAdditionalData { get; }

        public GetAllProjectsQuery(bool includeAdditionalData = false)
        {
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
