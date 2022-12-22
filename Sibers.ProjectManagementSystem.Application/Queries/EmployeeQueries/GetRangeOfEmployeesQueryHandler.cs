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

namespace Sibers.ProjectManagementSystem.Application.Queries.EmployeeQueries
{
    public class GetRangeOfEmployeesQueryHandler : IRequestHandler<GetRangeOfEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Employee, EmployeeDto> _mapper;

        public GetRangeOfEmployeesQueryHandler(ProjectManagementSystemContext context, IMapper<Employee, EmployeeDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetRangeOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Employee>? employees = new List<Employee>();
                Employee? employee;
                foreach (var id in request.EmployeesIds)
                {
                    employee = await GetEmployeeAsync(id, request.IncludeAdditionalData);
                    if (employee == null)
                        return Result<IEnumerable<EmployeeDto>>.NotFound($"No such employee with id: {id}");
                    else
                        employees.Add(employee);
                }
                return Result<IEnumerable<EmployeeDto>>.Success(employees.Select(e => _mapper.Map(e)));               
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<IEnumerable<EmployeeDto>>(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Employee?> GetEmployeeAsync(int id, bool includeAdditionalData)
        {
            Employee? employee;
            if (includeAdditionalData)
            {
                employee = await _context.Employees
                    .IncludeProjects()
                    .IncludeTasks()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            else
            {
                employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            return employee;
        }
    }
}
