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

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class UpdateProjectsDataCommandHandler : IRequestHandler<UpdateProjectsDataCommand, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;
        private IMapper<Project, ProjectDto> _mapper;

        public UpdateProjectsDataCommandHandler(ProjectManagementSystemContext context, IMapper<Project, ProjectDto> mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<ProjectDto>> Handle(UpdateProjectsDataCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project? project = await _context.Projects
                    .FirstOrDefaultAsync(p => p.Id == request.Project.Id, cancellationToken);
                if (project == null)
                    return Result<ProjectDto>.NotFound($"No such project with id: {request.Project.Id}");
                project.ChangePriority(new Priority(request.Project.Priority));
                project.ChangeContractorCompanyName(request.Project.NameOfTheContractorCompany);
                project.ChangeCustomerCompanyName(request.Project.NameOfTheCustomerCompany);
                project.ChangeName(request.Project.Name);
                project.ChangeStartDate(request.Project.StartDate);
                project.ChangeEndDate(request.Project.EndDate);
                await _context.SaveEntitiesAsync(cancellationToken);
                return Result.Success<ProjectDto>(_mapper.Map(project));
            }
            catch (DomainException ex)
            {
                return DomainExceptionHandler.Handle<ProjectDto>(ex);
            }
            catch (Exception e)
            {
                return Result<ProjectDto>.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
