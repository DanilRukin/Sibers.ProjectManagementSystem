using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.Application.Services.Mappers
{
    public class TaskToTaskDtoMapper : IMapper<Task, TaskDto>
    {
        public TaskDto Map(Task source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            TaskDto result = new TaskDto
            {
                Id = source.Id,
                ProjectId = source.ProjectId,
                AuthorEmployeeId = source.AuthorEmployeeId,
                ContractorEmployeeId = source.ContractorEmployeeId,
                Name = source.Name,
                Description = source.Description,
                Priority = source.Priority.Value,
                TaskStatus = source.TaskStatus,
            };
            return result;
        }
    }
}
