using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services;
using Sibers.ProjectManagementSystem.DataAccess;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
    {
        private ProjectManagementSystemContext _context;

        public CreateProjectCommandHandler(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ProjectFactory projectFactory = new ProjectFactory();
                Project project = projectFactory.CreateProject(request.ProjectDto.Name,
                    request.ProjectDto.StartDate,
                    request.ProjectDto.EndDate,
                    request.ProjectDto.NameOfTheContractorCompany,
                    request.ProjectDto.NameOfTheCustomerCompany,
                    request.ProjectDto.Priority);
                _context.Projects.Add(project);
                await _context.SaveEntitiesAsync(cancellationToken);
                request.ProjectDto.Id = project.Id;
                return Result<ProjectDto>.Success(request.ProjectDto);
            }
            catch (DomainException domainException)
            {
                return DomainExceptionHandler.Handle<ProjectDto>(domainException);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
