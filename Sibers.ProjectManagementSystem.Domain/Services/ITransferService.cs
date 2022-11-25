using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public interface ITransferService
    {
        Task AddEmployeeToProject(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task RemoveEmployeeFromProject(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task PromoteEmployeeToManager(int employeeId, int projectId, CancellationToken cancellationToken = default);
        Task DemoteManagerToEmployee(int projectId, CancellationToken cancellationToken = default);
        Task FireManager(int projectId, CancellationToken cancellationToken = default);
        Task TransferEmployeeToAnotherProject(int employeeId, int currentProjectId, int futureProjectId, CancellationToken cancellationToken);
    }
}
