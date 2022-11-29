using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
