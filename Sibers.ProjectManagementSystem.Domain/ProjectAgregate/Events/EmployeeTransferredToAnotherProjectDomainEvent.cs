using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class EmployeeTransferredToAnotherProjectDomainEvent : DomainEvent
    {
        public int OldProjectId { get; }
        public int NewProjectId { get; }
        public int EmployeeId { get; }

        public EmployeeTransferredToAnotherProjectDomainEvent(int oldProjectId, int newProjectId, int employeeId)
        {
            OldProjectId = oldProjectId;
            NewProjectId = newProjectId;
            EmployeeId = employeeId;
        }
    }
}
