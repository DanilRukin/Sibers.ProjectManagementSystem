using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto EmployeeDto { get; private set; }

        public CreateEmployeeCommand(EmployeeDto employeeDto)
        {
            EmployeeDto = employeeDto ?? throw new ArgumentNullException(nameof(employeeDto));
        }
    }
}
