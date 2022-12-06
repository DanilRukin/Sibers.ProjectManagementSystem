using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public class EmployeeFactory
    {
        public Employee CreateEmployee(string email, string firstName, string lastName, string patronymic = "")
        {
            return new Employee(0,
                new PersonalData(firstName, lastName, patronymic),
                new Email(email));
        }
    }
}
