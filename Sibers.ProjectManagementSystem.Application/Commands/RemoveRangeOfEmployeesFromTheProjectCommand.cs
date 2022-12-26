using MediatR;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application.Commands
{
    public class RemoveRangeOfEmployeesFromTheProjectCommand : IRequest<IResult>
    {
        public IEnumerable<int> EmployeesIds { get; private set; }
        public int ProjectId { get; private set; }

        public RemoveRangeOfEmployeesFromTheProjectCommand(IEnumerable<int> employeesIds, int projectId)
        {
            EmployeesIds = employeesIds ?? throw new ArgumentNullException(nameof(employeesIds));
            ProjectId = projectId;
        }
    }
}
