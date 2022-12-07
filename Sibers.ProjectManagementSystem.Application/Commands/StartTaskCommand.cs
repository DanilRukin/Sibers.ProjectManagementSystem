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
    public class StartTaskCommand : IRequest<IResult>
    {
        [DataMember]
        public Guid TaskId { get; private set; }
        [DataMember]
        public int EmployeeId { get; private set; }
        [DataMember]
        public int ProjectId { get; private set; }

        public StartTaskCommand(Guid taskId, int employeeId, int projectId)
        {
            TaskId = taskId;
            EmployeeId = employeeId;
            ProjectId = projectId;
        }
    }
}
