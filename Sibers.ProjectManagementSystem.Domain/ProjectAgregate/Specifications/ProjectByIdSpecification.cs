using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications
{
    public class ProjectByIdSpecification : Specification<Project>
    {
        private int _id;

        public ProjectByIdSpecification(int id)
        {
            _id = id;
        }

        public override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Id == _id;
        }
    }
}
