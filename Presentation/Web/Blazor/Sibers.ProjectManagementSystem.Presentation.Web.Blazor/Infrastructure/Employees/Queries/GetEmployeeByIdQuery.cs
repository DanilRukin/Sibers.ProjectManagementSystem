using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public int EmployeeId { get; private set; }
        public bool IncludeAdditionalData { get; private set; }

        public GetEmployeeByIdQuery(int employeeId, bool includeAdditionalData)
        {
            EmployeeId = employeeId;
            IncludeAdditionalData = includeAdditionalData;
        }
    }
}
