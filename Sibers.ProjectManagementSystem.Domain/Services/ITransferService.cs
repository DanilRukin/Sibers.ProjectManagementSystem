using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public interface ITransferService
    {
        void TransferEmployeeToAnotherProject(Employee employee, Project currentProject, Project futureProject);
    }
}
