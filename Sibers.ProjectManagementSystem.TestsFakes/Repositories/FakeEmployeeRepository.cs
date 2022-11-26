using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.TestsFakes.Repositories
{
    public class FakeEmployeeRepository : FakeGenericRepository<Employee>
    {
        public FakeEmployeeRepository(List<Employee> storage)
        {
            _entities = storage;
        }
    }
}
