using Sibers.ProjectManagementSystem.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application
{
    public interface ITransferService
    {
        Task<IResult> AddEmployeeToProjectAsync(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task<IResult> RemoveEmployeeFromProjectAsync(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task<IResult> PromoteEmployeeToManagerAsync(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task<IResult> DemoteManagerToEmployeeAsync(int projectId, string reason = "", CancellationToken cancellationToken = default);
        Task<IResult> FireManagerAsync(int projectId, string reason = "", CancellationToken cancellationToken = default);
        Task<IResult> TransferEmployeeToAnotherProjectAsync(int employeeId, int currentProjectId, int futureProjectId, CancellationToken cancellationToken = default);
    }
}
