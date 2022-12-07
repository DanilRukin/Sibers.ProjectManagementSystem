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
    public class CompleteTaskCommand : IRequest<IResult>
    {
        [DataMember]
        public Guid TaskId { get; private set; }
        [DataMember]
        public int ProjectId { get; private set; }
        [DataMember]
        public int EmployeeId { get; private set; }

        public CompleteTaskCommand(Guid taskId, int projectId, int employeeId)
        {
            TaskId = taskId;
            ProjectId = projectId;
            EmployeeId = employeeId;
        }

        public CompleteTaskCommand()
        {
        }
    }
}
