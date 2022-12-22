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
    internal class GetRangeOfProjectsQueryHandler : IRequestHandler<GetRangeOfProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Project, ProjectDto> _mapper;

        public GetRangeOfProjectsQueryHandler(ProjectManagementSystemContext context, IMapper<Project, ProjectDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetRangeOfProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Project>? projects = new List<Project>();
                Project? project;
                foreach (var id in request.ProjectsIds)
                {
                    project = await GetProject(id, request.IncludeAdditionalData);
                    if (project == null)
                        return Result<IEnumerable<ProjectDto>>.NotFound($"No such project with id: {id}");
                    else
                        projects.Add(project);
                }
                return Result<IEnumerable<ProjectDto>>.Success(projects.Select(p => _mapper.Map(p)));
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<IEnumerable<ProjectDto>>(ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Project?> GetProject(int id, bool includeAdditionalData)
        {
            Project? result;
            if (includeAdditionalData)
                result = await _context.Projects
                    .IncludeEmployees()
                    .IncludeTasks()
                    .FirstOrDefaultAsync(p => p.Id == id);
            else
                result = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }
    }
}
