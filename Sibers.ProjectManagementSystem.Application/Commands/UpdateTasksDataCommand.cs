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
    public class UpdateTasksDataCommand : IRequest<Result<TaskDto>>
    {
        public TaskDto Task { get; private set; }

        public UpdateTasksDataCommand(TaskDto task)
        {
            Task = task ?? throw new ArgumentNullException(nameof(task));
        }
    }
}
