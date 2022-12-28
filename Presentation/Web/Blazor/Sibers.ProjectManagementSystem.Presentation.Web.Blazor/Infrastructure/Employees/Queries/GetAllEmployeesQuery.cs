using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<Result<IEnumerable<EmployeeDto>>>
    {
        public bool IncludeAdditionalData { get; private set; }

        public GetAllEmployeesQuery(bool includeAdditionalData)
        {
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
