using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class CreateProjectCommand : IRequest<Result<ProjectDto>>
    {
        public ProjectDto ProjectDto { get; set; }

        public CreateProjectCommand(ProjectDto projectDto)
        {
            ProjectDto = projectDto ?? throw new ArgumentNullException(nameof(projectDto));
        }
    }
}
