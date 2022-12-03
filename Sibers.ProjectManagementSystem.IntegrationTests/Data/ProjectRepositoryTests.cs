using Sibers.ProjectManagementSystem.DataAccess.Repositories;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.IntegrationTests.Data
{
    public class ProjectRepositoryTests : BaseEfRepoTestFixture<IProjectRepository, Project>
    {
        public ProjectRepositoryTests() : base() { }

        protected override IProjectRepository GetRepository()
        {
            return new ProjectRepository(_context);
        }
    }
}
