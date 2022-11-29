using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events
{
    public class TaskCreatedDomainEvent : DomainEvent
    {
        public int ProjectId { get; }
        public Task Task { get; }
        public int EmployeeId { get; }

        public TaskCreatedDomainEvent(int projectId, Task task, int employeeId)
        {
            ProjectId = projectId;
            Task = task;
            EmployeeId = employeeId;
        }
    }
}
