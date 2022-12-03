using Microsoft.EntityFrameworkCore;
using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private ProjectManagementSystemContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProjectRepository(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Project> AddAsync(Project entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IEnumerable<Project>> AddRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            await _context.AddRangeAsync(entities, cancellationToken);
            return entities;
        }

        public Task DeleteAsync(Project entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                _context.Remove(entity);
            });
        }

        public Task DeleteRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                _context.RemoveRange(entities);
            });
        }

        public async Task<Project> Find(Specification<Project> specification)
        {
            return await _context.Projects
                .Where(specification.ToExpression())
                .FirstOrDefaultAsync();
        }

        public void DoInclude(Specification<Project> specification)
        {
            var query = _context.Projects
                .Include(p => p.Employees)
                .ThenInclude(e => e.ExecutableTasks)
                .Include(p => p.Employees)
                .ThenInclude(e => e.CreatedTasks)
                .Where(specification.ToExpression())
                .ToList();
        }

        public async Task<IEnumerable<Project>> FindAll(Specification<Project> specification)
        {
            return await _context.Projects
                .Where(specification.ToExpression())
                .ToListAsync();
        }

        public Task UpdateAsync(Project entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                _context.Update(entity);
            });
        }

        public Task UpdateRangeAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                _context.Update(entities);
            });
        }
    }
}
