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
    public class DemoteManagerToTheEmployeeCommandHandler : IRequestHandler<DemoteManagerToTheEmployeeCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public DemoteManagerToTheEmployeeCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(DemoteManagerToTheEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);  // employees were auto included
                if (project == null)
                    return Result.NotFound($"No such project with id: {request.ProjectId}");
                project.DemoteManagerToEmployee(request.Reason);
                _context.Projects.Update(project);
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
