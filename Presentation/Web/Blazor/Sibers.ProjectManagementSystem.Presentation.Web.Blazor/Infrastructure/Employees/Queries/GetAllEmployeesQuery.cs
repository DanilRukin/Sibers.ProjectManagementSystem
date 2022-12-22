using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public bool IncludeAdditionalData { get; private set; }

        public GetAllEmployeesQuery(bool includeAdditionalData)
        {
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
