﻿using MediatR;
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
    public class DeleteProjectCommand : IRequest<IResult>
    {
        [DataMember]
        public int ProjectId { get; private set; }

        public DeleteProjectCommand(int projectId)
        {
            ProjectId = projectId;
        }

        public DeleteProjectCommand()
        {
        }
    }
}
