using MediatR;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class AddRangeOfEmployeesCommand : IRequest<IResult>
    {
        public IEnumerable<int> EmployeesIds { get; private set; }
        public int ProjectId { get; private set; }

        public AddRangeOfEmployeesCommand(IEnumerable<int> employeesIds, int projectId)
        {
            EmployeesIds = employeesIds ?? throw new ArgumentNullException(nameof(employeesIds));
            ProjectId = projectId;
        }
    }
}
