using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.TestsFakes.Repositories
{
    public class FakeGenericRepository<T> : IRepository<T> where T : class, IAgregateRoot
    {
        protected List<T> _entities = new List<T>();
        public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!_entities.Contains(entity))
                    _entities.Add(entity);
                return entity;
            });
        }

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var item in entities)
                {
                    if (!_entities.Contains(item))
                        _entities.Add(item);
                }
                return entities;
            });
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                _entities.Remove(entity);
                return entity;
            });
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var item in entities)
                {
                    _entities.Remove(item);
                }
                return entities;
            });
        }

        public Task<T> Find(Specification<T> specification)
        {
            return Task.Factory.StartNew(() =>
            {
                T result = _entities.AsQueryable().Where(specification.ToExpression()).First();
                return result;
            });
        }

        public Task<IEnumerable<T>> FindAll(Specification<T> specification)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = _entities.AsQueryable().Where(specification.ToExpression()).AsEnumerable();
                return result;
            });
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                if (_entities.Contains(entity))
                    _entities.Remove(entity);
                _entities.Add(entity);
                return entity;
            });
        }

        public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var entity in entities)
                {
                    if (_entities.Contains(entity))
                        _entities.Remove(entity);
                    _entities.Add(entity);
                }
                return entities;
            });
        }
    }
}
