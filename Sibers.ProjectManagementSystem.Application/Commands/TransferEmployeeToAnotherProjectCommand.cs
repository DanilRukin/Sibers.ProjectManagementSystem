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
    public class TransferEmployeeToAnotherProjectCommand : IRequest<IResult>
    {
        [DataMember]
        public int FromProjectId { get; private set; }
        [DataMember]
        public int ToProjectId { get; private set; }
        [DataMember]
        public int EmployeeId { get; private set; }

        public TransferEmployeeToAnotherProjectCommand(int fromProjectId, int toProjectId, int employeeId)
        {
            FromProjectId = fromProjectId;
            ToProjectId = toProjectId;
            EmployeeId = employeeId;
        }

        public TransferEmployeeToAnotherProjectCommand()
        {
        }
    }
}
