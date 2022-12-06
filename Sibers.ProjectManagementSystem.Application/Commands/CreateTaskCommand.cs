using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateTaskCommand : IRequest<Result<TaskDto>>
    {
        public int ProjectId { get; set; }
        public int AuthorId { get; set; }
        public TaskDto TaskDto { get; set; }

        public CreateTaskCommand(int projectId, int authorId, TaskDto taskDto)
        {
            ProjectId = projectId;
            AuthorId = authorId;
            TaskDto = taskDto ?? throw new ArgumentNullException(nameof(taskDto));
        }
    }
}
