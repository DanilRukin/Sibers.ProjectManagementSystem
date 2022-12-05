using Sibers.ProjectManagementSystem.Domain;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.IntegrationTests.Data.EfCoreContextTests
{
    public class DataAccessTests : BaseEfTestFixture
    {
        [Fact]
        public void AddProject_ProjectShouldBeAdded()
        {
            var context = GetClearContext();
            Project project = GetClearProjectWithId(0);
            context.Projects.Add(project);
            context.SaveChanges();

            Project result = context.Projects.FirstOrDefault();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public void AddEmployee_EmployeeShouldBeAdded()
        {
            var context = GetClearContext();
            Employee employee = GetClearEmployeeWithId(0);
            context.Employees.Add(employee);
            context.SaveChanges(true);

            Employee result = context.Employees.FirstOrDefault();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public void AddManagerToTheProject_ManagerShouldBeAdded()
        {
            var context = GetClearContext();
            Project project = GetClearProjectWithId(0);
            Employee employee = GetClearEmployeeWithId(0);
            context.Employees.Add(employee);
            context.SaveChanges();

            context.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Employee existingEmployee = context.Employees.FirstOrDefault();  // fetch an existing employee
            Assert.NotNull(existingEmployee);
            Assert.NotSame(employee, existingEmployee);

            project.AddEmployee(existingEmployee);
            project.PromoteEmployeeToManager(existingEmployee);
            context.Projects.Add(project);
            context.SaveChanges();

            context.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Project existingProject = context.Projects.FirstOrDefault();
            Assert.NotNull(existingProject);
            Assert.NotSame(project, existingProject);
            Assert.NotNull(existingProject.Manager);
            Assert.Single(context.EmployeesOnProjects);
            var eop = context.EmployeesOnProjects.First();
            Assert.Equal(EmployeeRoleOnProject.Manager, eop.Role);
        }

        [Fact]
        public void AddProject_EmployeeWasInSystem_ProjectShouldContainsEmployee()
        {
            var context = GetClearContext();
            Project project = GetClearProjectWithId(0);
            Employee existingEmployee = GetClearEmployeeWithId(0);
            context.Employees.Add(existingEmployee);
            context.SaveChanges(true);
            existingEmployee = context.Employees.FirstOrDefault();  // employee was in system

            project.AddEmployee(existingEmployee);  // adding employee to the project
            context.Projects.Add(project);
            context.SaveChanges();
            Project result = context.Projects.FirstOrDefault();

            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Contains(existingEmployee, result.Employees);
        }

        [Fact]
        public void UpdateProject_AddAnEmployeeToProjectAfterItWasSaved_NewProjectAndDetachedEmployeeShouldBeUpdated()
        {
            var context = GetClearContext();
            Project project = GetClearProjectWithId(0);
            context.Projects.Add(project);
            context.SaveChanges();

            Employee existingEmployee = GetClearEmployeeWithId(0);
            context.Employees.Add(existingEmployee);
            context.SaveChanges(true);

            context.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Project newProject = context.Projects.FirstOrDefault();  // fetch new project, newProject not tracking
            Assert.NotNull(newProject);
            Assert.NotSame(project, newProject);

            string name = "new name";  // new name for the project
            newProject.ChangeName(name);

            context.Entry(existingEmployee).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Employee employee = context.Employees.FirstOrDefault();
            newProject.AddEmployee(employee);

            context.Update(newProject);
            context.SaveChanges();

            Project updatedProject = context.Projects.FirstOrDefault(p => p.Name == name);
            Assert.NotNull(updatedProject);
            Assert.NotEqual(project.Name, updatedProject.Name);
            Assert.Equal(project.Id, updatedProject.Id);
            Assert.Empty(project.Employees);
            Assert.Contains(employee, updatedProject.Employees);
            Assert.Contains(newProject, employee.Projects);
            Assert.Empty(existingEmployee.Projects);
            Assert.Single(context.Projects);
            Assert.Single(context.Employees);
            Assert.Single(context.EmployeesOnProjects);
        }

        [Fact]
        public void GetProject_EmployeeWasAddedToTheProject_WillGetDetachedProject_ProjectShouldContainsAnEmployeeBecauseOfAutoInclude()
        {
            var context = GetClearContext();
            Project project = GetClearProjectWithId(0);
            Employee existingEmployee = GetClearEmployeeWithId(0);
            context.Employees.Add(existingEmployee);
            context.SaveChanges();
            context.Entry(existingEmployee).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Employee employee = context.Employees.FirstOrDefault();  // fetch new employee from db
            Assert.NotNull(employee);
            Assert.NotSame(existingEmployee, employee);

            project.AddEmployee(employee);
            context.Projects.Add(project);
            context.SaveChanges();

            context.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            Project newProject = context.Projects.FirstOrDefault(); // fetch new project

            Assert.NotNull(newProject);
            Assert.NotSame(project, newProject);
            Assert.Contains(employee, newProject.Employees);  // employee should be auto included
            Assert.Single(context.EmployeesOnProjects);
        }

        [Fact]
        public void Delete_DeleteProject_ProjectShouldBeDeleted()
        {
            var contex = GetClearContext();
            Project project = GetClearProjectWithId(0);
            contex.Projects.Add(project);
            contex.SaveChanges();
            contex.Entry(project).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            contex.Projects.Remove(project);
            contex.SaveChanges();

            Project result = contex.Projects.FirstOrDefault();
            Assert.Null(result);
            Assert.Empty(contex.Projects);
        }

        private Project GetClearProjectWithId(int id)
        {
            DateTime _startDate = DateTime.Today;
            DateTime _endDate = _startDate.AddDays(1);
            Project project = new Project(id, "ProjectName",
                _startDate,
                _endDate,
                Priority.Default(),
                "CustomerCompany",
                "ContractorCompany");
            return project;
        }

        private Employee GetClearEmployeeWithId(int id)
        {
            return new Employee(id,
                new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("Employee@gmail.com"));
        }
    }
}
