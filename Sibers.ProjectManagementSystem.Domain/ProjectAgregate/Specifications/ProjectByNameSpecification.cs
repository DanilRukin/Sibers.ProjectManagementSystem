using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications
{
    public class ProjectByNameSpecification : Specification<Project>
    {
        public string _name;
        public ProjectByNameSpecification(string name)
        {
            _name = name;
        }
        public override Expression<Func<Project, bool>> ToExpression()
        {
            return project => project.Name == _name;
        }
    }
}
