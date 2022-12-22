using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetRangeOfEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
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
