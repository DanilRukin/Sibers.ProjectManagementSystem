using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class UpdateTasksDataCommandHandler : IRequestHandler<UpdateTasksDataCommand, Result<TaskDto>>
    {
        private ProjectManagementSystemContext _context;

        public UpdateTasksDataCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<TaskDto>> Handle(UpdateTasksDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.Task.Id);
                if (task == null)
                    return Result<TaskDto>.NotFound($"No such task with id: {request.Task.Id}");
                task.ChangeName(request.Task.Name);
                task.ChangePriority(new Domain.ProjectAgregate.Priority(request.Task.Priority));
                task.ChangeDescription(request.Task.Description);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result<TaskDto>.Success(request.Task);
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<TaskDto>(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
