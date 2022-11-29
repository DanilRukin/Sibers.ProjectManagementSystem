using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectManagementSystemContext _context;

        public ProjectRepository(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Task<Project> AddAsync(Project entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> AddRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Project entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Project> Find(Specification<Project> specification)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Project>> FindAll(Specification<Project> specification)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Project entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
