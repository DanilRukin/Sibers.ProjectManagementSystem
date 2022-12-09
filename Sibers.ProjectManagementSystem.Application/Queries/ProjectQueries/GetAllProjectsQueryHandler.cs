﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain;
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
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private ProjectManagementSystemContext _context;
        IMapper<Project, ProjectDto> _mapper;

        public GetAllProjectsQueryHandler(ProjectManagementSystemContext context, IMapper<Project, ProjectDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.IncludeAdditionalData)
                {
                    IEnumerable<Project> projects = await _context.Projects
                        .IncludeTasks()
                        .IncludeEmployees()
                        .ToListAsync(cancellationToken);
                    if (projects.Any())
                        return Result.Success<IEnumerable<ProjectDto>>(projects.Select(p => _mapper.Map(p)));
                }
                else
                {
                    IEnumerable<Project> projects = await _context.Projects
                        .ToListAsync(cancellationToken);
                    if (projects.Any())
                        return Result.Success<IEnumerable<ProjectDto>>(projects.Select(p => _mapper.Map(p)));
                }
                return Result<IEnumerable<ProjectDto>>.NotFound();
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
    }
}