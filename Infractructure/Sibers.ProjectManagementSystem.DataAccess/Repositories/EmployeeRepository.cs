using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ProjectManagementSystemContext _context;

        public EmployeeRepository(ProjectManagementSystemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IUnitOfWork UnitOfWork => _context;

        public Task<Employee> AddAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> AddRangeAsync(IEnumerable<Employee> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<Employee> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> Find(Specification<Employee> specification)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> FindAll(Specification<Employee> specification)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<Employee> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
