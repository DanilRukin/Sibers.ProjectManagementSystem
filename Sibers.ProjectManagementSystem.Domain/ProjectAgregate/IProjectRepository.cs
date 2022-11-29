using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public interface IProjectRepository : IRepository<Project>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
