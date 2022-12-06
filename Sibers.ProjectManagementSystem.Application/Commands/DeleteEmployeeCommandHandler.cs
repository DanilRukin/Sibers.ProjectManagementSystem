using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
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
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, IResult>
    {
        private ProjectManagementSystemContext _context;

        public DeleteEmployeeCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IResult> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? toDelete = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);
                if (toDelete != null)
                {
                    _context.Employees.Remove(toDelete);
                    _context.SaveChanges();
                    return Result.Success();
                }
                else
                {
                    return Result.NotFound($"No such employee with id: {request.EmployeeId}");
                }
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
