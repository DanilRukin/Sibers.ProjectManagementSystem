using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.TaskAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public DeleteProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? toDelete = await _context.Projects
                    .IncludeEmployees()
                    .IncludeTasks()
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (toDelete != null)
                {                   
                    Work[] tasks = toDelete.Tasks.ToArray();
                    Employee? manager = toDelete.Manager;
                    if (manager != null)
                        toDelete.FireManager("Project was deleted");
                    Employee[] employees = toDelete.Employees.ToArray();
                    foreach (Employee employee in employees)
                    {                       
                        toDelete.RemoveEmployee(employee);
                    }
                    foreach (var task in tasks)
                    {
                        toDelete.RemoveTask(task, manager == null ? 0 : manager.Id);
                    }
                    _context.Projects.Remove(toDelete);
                    await _context.SaveEntitiesAsync(cancellationToken);
                    return Result.Success();
                }
                else
                {
                    return Result.NotFound($"Project with id: {request.ProjectId} was not find");
                }
            }
            catch (DomainException domainException)
            {
                return DomainExceptionHandler.Handle(domainException);
            }
            catch (Exception e)
            {
                return Result.Error($"Something was wron during project deleting process. Reason: {e.Message}");
            }
        }
    }
}
