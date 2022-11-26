using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.TestsFakes.Repositories
{
    public class FakeProjectRepository : FakeGenericRepository<Project>
    {
        public FakeProjectRepository(List<Project> storage)
        {
            this._entities = storage;
        }
    }
}
