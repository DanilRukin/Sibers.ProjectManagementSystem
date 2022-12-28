using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
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
