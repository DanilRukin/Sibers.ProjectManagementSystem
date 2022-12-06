using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        public EmployeeDto EmployeeDto { get; set; }

        public CreateEmployeeCommand(EmployeeDto employeeDto)
        {
            EmployeeDto = employeeDto ?? throw new ArgumentNullException(nameof(employeeDto));
        }
    }
}
