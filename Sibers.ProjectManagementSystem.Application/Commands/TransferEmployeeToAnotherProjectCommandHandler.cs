using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class TransferEmployeeToAnotherProjectCommandHandler : IRequestHandler<TransferEmployeeToAnotherProjectCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public TransferEmployeeToAnotherProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(TransferEmployeeToAnotherProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? currentProject = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.FromProjectId, cancellationToken);  // employees were auto included
                if (currentProject == null)
                    return Result.NotFound($"No such project with id: {request.FromProjectId}");
                Project? futureProject = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.ToProjectId, cancellationToken);  // employees were auto included
                if (futureProject == null)
                    return Result.NotFound($"No such project with id: {request.ToProjectId}");
                Employee? employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);  // projects were auto included
                if (employee == null)
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");

                TransferService transferService = new TransferService();
                transferService.TransferEmployeeToAnotherProject(employee, currentProject, futureProject);

                _context.Projects.UpdateRange(currentProject, futureProject);
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
