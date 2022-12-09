using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
    {
        private ProjectManagementSystemContext _context;

        public CreateTaskCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .IncludeTasks()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId);
                if (project == null)
                    return Result<TaskDto>.NotFound($"No project with id: {request.ProjectId}");
                Employee? author = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.AuthorId);
                if (author == null)
                    return Result<TaskDto>.NotFound($"No employee with id: {request.AuthorId}");
                var task = author.CreateTask(project, request.TaskDto.Name, request.TaskDto.Priority);
                _context.Projects.Update(project);
                _context.Employees.Update(author);
                //_context.Tasks.Add(task);  // ??
                await _context.SaveEntitiesAsync(cancellationToken);
                request.TaskDto.ProjectId = task.ProjectId;
                request.TaskDto.AuthorEmployeeId = task.AuthorEmployeeId;               
                return Result<TaskDto>.Success(request.TaskDto);
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
