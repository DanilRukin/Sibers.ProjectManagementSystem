using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications
{
    public abstract class Specification<T> : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();
        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate.Invoke(entity);
        }

        public Specification<T> And(Specification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);

        public Specification<T> Not(Specification<T> other) => new NotSpecification<T>(other);
    }
}
