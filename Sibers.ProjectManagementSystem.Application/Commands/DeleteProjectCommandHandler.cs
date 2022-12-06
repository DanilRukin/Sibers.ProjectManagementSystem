using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
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
                    .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                if (toDelete != null)
                {
                    _context.Projects.Remove(toDelete);
                    _context.SaveChanges();
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
