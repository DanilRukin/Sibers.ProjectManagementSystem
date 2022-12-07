using MediatR;
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
    public class AddEmployeeToTheProjectCommandHandler : IRequestHandler<AddEmployeeToTheProjectCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public AddEmployeeToTheProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(AddEmployeeToTheProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);  // employees were auto included
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                Employee? employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);  // projects were auto included
                if (employee == null)
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");
                project.AddEmployee(employee);
                _context.Projects.Update(project);
                _context.Employees.Update(employee);
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
