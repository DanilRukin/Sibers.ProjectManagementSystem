using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Queries.EmployeeQueries
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Employee, EmployeeDto> _mapper;

        public GetAllEmployeesQueryHandler(ProjectManagementSystemContext context, IMapper<Employee, EmployeeDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Employee>? employees;
                if (request.IncludeAdditionalData)
                {
                    employees = await _context.Employees
                        .IncludeTasks()
                        .IncludeProjects()
                        .ToListAsync(cancellationToken);
                }
                else
                {
                    employees = await _context.Employees.ToListAsync(cancellationToken);
                }
                if (employees == null)
                    return Result<IEnumerable<EmployeeDto>>.NotFound("No employees in database");
                return Result<IEnumerable<EmployeeDto>>.Success(employees.Select(e => _mapper.Map(e)));
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<IEnumerable<EmployeeDto>>(ex);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<EmployeeDto>>.Error($"Something was wrong during the getting all" +
                    $" employees process. Reason {e.Message}");
            }
        }
    }
}
