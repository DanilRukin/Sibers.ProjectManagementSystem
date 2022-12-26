using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Commands
{
    public class UpdateEmployeesDataCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto Employee { get; private set; }

        public UpdateEmployeesDataCommand(EmployeeDto employee)
        {
            Employee = employee ?? throw new ArgumentNullException(nameof(employee));
        }
    }
}
