﻿using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public CompleteTaskCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .IncludeTasks()
                    .IncludeEmployees()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                Employee? employee = await _context.Employees
                    .IncludeProjects()
                    .IncludeExecutableTasks()
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
                if (employee == null)
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
                if (task == null)
                    return Result.NotFound($"No such task with id: {request.TaskId}");

                employee.CompleteTask(project, task);
                _context.Employees.Update(employee);
                _context.Tasks.Update(task);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success();
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}