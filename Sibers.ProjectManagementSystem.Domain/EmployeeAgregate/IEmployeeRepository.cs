using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
