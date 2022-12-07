using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    [DataContract]
    public class CreateEmployeeCommand : IRequest<Result<EmployeeDto>>
    {
        [DataMember]
        public EmployeeDto EmployeeDto { get; private set; }

        public CreateEmployeeCommand(EmployeeDto employeeDto)
        {
            EmployeeDto = employeeDto ?? throw new ArgumentNullException(nameof(employeeDto));
        }

        public CreateEmployeeCommand()
        {
        }
    }
}
