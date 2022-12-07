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
    public class PromoteEmployeeToManagerCommand : IRequest<IResult>
    {
        [DataMember]
        public int ProjectId { get; private set; }
        [DataMember]
        public int EmployeeId { get; private set; }

        public PromoteEmployeeToManagerCommand(int projectId, int employeeId)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
        }

        public PromoteEmployeeToManagerCommand()
        {
        }
    }
}
