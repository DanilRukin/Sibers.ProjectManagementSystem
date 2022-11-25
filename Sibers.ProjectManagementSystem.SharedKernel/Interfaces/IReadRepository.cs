using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.Interfaces
{
    public interface IReadRepository<T> where T : class, IAgregateRoot
    {
        Task<T> Find(Specification<T> specification);
        Task<IEnumerable<T>> FindAll(Specification<T> specification);
    }
}
