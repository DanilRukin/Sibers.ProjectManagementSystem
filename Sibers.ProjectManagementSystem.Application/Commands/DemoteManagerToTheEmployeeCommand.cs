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
    public class DemoteManagerToTheEmployeeCommand : IRequest<IResult>
    {
        [DataMember]
        public int ProjectId { get; private set; }
        [DataMember]
        public string Reason { get; private set; }

        public DemoteManagerToTheEmployeeCommand(int projectId, string reason)
        {
            ProjectId = projectId;
            Reason = reason;
        }

        public DemoteManagerToTheEmployeeCommand()
        {
        }
    }
}
