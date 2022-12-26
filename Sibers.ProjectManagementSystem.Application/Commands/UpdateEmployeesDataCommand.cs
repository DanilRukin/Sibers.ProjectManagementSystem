using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
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
