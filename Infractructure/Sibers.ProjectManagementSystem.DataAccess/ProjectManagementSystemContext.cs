using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.TaskAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sibers.ProjectManagementSystem.DataAccess
{
    public class ProjectManagementSystemContext : DbContext, IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        private IDomainEventDispatcher _domainEventDispatcher;

        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Domain.TaskAgregate.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new TransactionException("Commit current transaction first");
            _transaction = Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new TransactionException("Begin transaction first");
            try
            {
                SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction == null)
                throw new TransactionException("Begin transaction first");
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async System.Threading.Tasks.Task SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            var events = ChangeTracker.Entries<IDomainObject>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();
            await _domainEventDispatcher.DispatchAndClearEvents(events);
        }
    }
}
