using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.Application.Dtos
{
    public class TaskDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public TaskStatus TaskStatus { get; set; }
        public int ProjectId { get; set; }
        public int? ContractorEmployeeId { get; set; }
        public int AuthorEmployeeId { get; set; }
    }
}
