using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class ContractorChangedDomainEvent : DomainEvent
    {
        public int ProjectId { get; }
        public Task Task { get; }
        public int NewContractor { get; }

        public ContractorChangedDomainEvent(int projectId, Task task, int newContractor)
        {
            ProjectId = projectId;
            Task = task;
            NewContractor = newContractor;
        }
    }
}
