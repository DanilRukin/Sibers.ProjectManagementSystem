using MediatR;
using Microsoft.Identity.Client;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Queries.ProjectQueries
{
    public class GetProjectByIdQuery : IRequest<Result<ProjectDto>>
    {
        public bool IncludeAdditionalData { get; private set; }
        public int ProjectId { get; private set; }

        public GetProjectByIdQuery(int projectId, bool includeAdditionalData = false)
        {
            ProjectId = projectId;
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
