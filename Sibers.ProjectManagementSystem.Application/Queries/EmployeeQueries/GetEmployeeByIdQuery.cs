using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Queries.EmployeeQueries
{
    public class GetEmployeeByIdQuery : IRequest<Result<EmployeeDto>>
    {
        public int EmployeeId { get; private set; }
        public bool IncludeAddtitionalData { get; private set; }

        public GetEmployeeByIdQuery(int employeeId, bool includeAddtitionalData)
        {
            EmployeeId = employeeId;
            IncludeAddtitionalData = includeAddtitionalData;
        }
    }
}
