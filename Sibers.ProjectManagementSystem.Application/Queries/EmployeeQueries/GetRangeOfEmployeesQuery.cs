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
    public class GetRangeOfEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public IEnumerable<int> EmployeesIds { get; private set; }
        public bool IncludeAdditionalData { get; private set; }

        public GetRangeOfEmployeesQuery(IEnumerable<int> employeesIds, bool includeAdditionalData)
        {
            EmployeesIds = employeesIds ?? throw new ArgumentNullException(nameof(employeesIds));
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
