using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.Services;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand ,Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;

        public CreateEmployeeCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                EmployeeFactory factory = new EmployeeFactory();
                Employee employee = factory.CreateEmployee(request.EmployeeDto.Email,
                    request.EmployeeDto.FirstName,
                    request.EmployeeDto.LastName,
                    request.EmployeeDto.Patronymic);
                _context.Employees.Add(employee);
                await _context.SaveEntitiesAsync(cancellationToken);
                request.EmployeeDto.Id = employee.Id;
                return Result<EmployeeDto>.Success(request.EmployeeDto);
            }
            catch (DomainException domainException)
            {
                return DomainExceptionHandler.Handle<EmployeeDto>(domainException);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
