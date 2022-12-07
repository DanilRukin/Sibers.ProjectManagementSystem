using MediatR;
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
    public class DeleteEmployeeCommand : IRequest<IResult>
    {
        [DataMember]
        public int EmployeeId { get; private set; }

        public DeleteEmployeeCommand(int employeeId)
        {
            EmployeeId = employeeId;
        }

        public DeleteEmployeeCommand()
        {
        }
    }
}
