using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
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

namespace Sibers.ProjectManagementSystem.Application.Queries.ProjectQueries
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Project, ProjectDto> _mapper;

        public GetProjectByIdQueryHandler(ProjectManagementSystemContext context, IMapper<Project, ProjectDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.IncludeAdditionalData)
                {
                    Project? result = await _context.Projects
                        .IncludeEmployees()
                        .IncludeTasks()
                        .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                    if (result == null)
                        return Result<ProjectDto>.NotFound($"No such project with id: {request.ProjectId}");

                    return Result.Success<ProjectDto>(_mapper.Map(result));
                }
                else
                {
                    Project? result = await _context.Projects
                        .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                    if (result == null)
                        return Result<ProjectDto>.NotFound($"No such project with id: {request.ProjectId}");

                    return Result.Success<ProjectDto>(_mapper.Map(result));
                }
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<ProjectDto>(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
