using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
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
    public class UpdateEmployeesDataCommandHandler : IRequestHandler<UpdateEmployeesDataCommand, Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Employee, EmployeeDto> _mapper;

        public UpdateEmployeesDataCommandHandler(ProjectManagementSystemContext context, IMapper<Employee, EmployeeDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<EmployeeDto>> Handle(UpdateEmployeesDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.Employee.Id, cancellationToken);
                if (employee == null)
                    return Result<EmployeeDto>.NotFound($"No such employee with id: {request.Employee.Id}");
                employee.ChangeFirstName(request.Employee.FirstName);
                employee.ChangeLastName(request.Employee.LastName);
                employee.ChangeEmail(request.Employee.Email);
                employee.ChangePatronymic(request.Employee.Patronymic);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success(_mapper.Map(employee));
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<EmployeeDto>(ex);
            }
            catch (Exception e)
            {
                return Result<EmployeeDto>.Error($"Something was wrong while updating employee's data. Reason: {e.Message}");
            }
        }
    }
}
