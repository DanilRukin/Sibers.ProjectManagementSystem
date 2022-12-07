using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    [DataContract]
    public class CreateProjectCommand : IRequest<Result<ProjectDto>>
    {
        [DataMember]
        public ProjectDto ProjectDto { get; private set; }

        public CreateProjectCommand(ProjectDto projectDto)
        {
            ProjectDto = projectDto ?? throw new ArgumentNullException(nameof(projectDto));
        }

        public CreateProjectCommand()
        {
        }
    }
}
