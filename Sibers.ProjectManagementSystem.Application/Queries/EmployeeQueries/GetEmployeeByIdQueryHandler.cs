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
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Employee, EmployeeDto> _mapper;

        public GetEmployeeByIdQueryHandler(ProjectManagementSystemContext context, IMapper<Employee, EmployeeDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Employee? employee;

                if (request.IncludeAddtitionalData)
                {
                    employee = await _context.Employees
                        .IncludeTasks()
                        .IncludeProjects()
                        .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
                }
                else
                {
                    employee = await _context.Employees
                        .FirstOrDefaultAsync(e => e.Id == request.EmployeeId);
                }
                if (employee == null)
                    return Result<EmployeeDto>.NotFound($"No such employee with id: {request.EmployeeId}");
                else
                    return Result<EmployeeDto>.Success(_mapper.Map(employee));
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<EmployeeDto>(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
