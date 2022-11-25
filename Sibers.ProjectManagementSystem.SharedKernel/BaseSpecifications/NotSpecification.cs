using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private Specification<T> _specification;
        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }
        public override Expression<Func<T, bool>> ToExpression()
        {
            var spec = _specification.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.Not(spec.Body);
            exprBody = (UnaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            return Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);
        }
    }
}
