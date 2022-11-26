using Sibers.ProjectManagementSystem.SharedKernel.BaseSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate.Specifications
{
    public class EmployeeByIdSpecification : Specification<Employee>
    {
        private int _id;

        public EmployeeByIdSpecification(int id)
        {
            _id = id;
        }

        public override Expression<Func<Employee, bool>> ToExpression()
        {
            return employee => employee.Id == _id;
        }
    }
}
