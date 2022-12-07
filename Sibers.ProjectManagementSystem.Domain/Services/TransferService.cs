using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.Services
{
    public class TransferService : ITransferService
    {
        public void TransferEmployeeToAnotherProject(Employee employee, Project currentProject, Project futureProject)
        {
            if (employee.Equals(currentProject.Manager))
                throw new DomainException($"You can not transfer this employee (id: {employee.Id}) from current project" +
                $" (id: {currentProject.Id}) because he is manager of this project");

            if (!currentProject.Employees.Contains(employee))
                throw new DomainException($"You can not transfer this employee (id: {employee.Id}) from current project" +
                $" (id: {currentProject.Id}) because he is not working on it");

            if (futureProject.Employees.Contains(employee) || employee.Equals(futureProject.Manager))
                throw new DomainException($"You can not transfer this employee (id: {employee.Id}) from current project" +
                $" (id: {currentProject.Id}) because he is already works on project you want to transfer (id: {futureProject.Id})");

            currentProject.RemoveEmployee(employee);

            futureProject.AddEmployee(employee);
        }
    }
}
