using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class ProjectTests
    {
        private Project _project;
        private DateTime _startDate;
        private DateTime _endDate;
        public ProjectTests()
        {
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
            _project = new Project(1, "Test project",
                _startDate,
                _endDate,
                new Priority(1),
                "CustomerCompany",
                "ContractorCompany");
        }

        [Fact]
        public void ChangeStartDate_StartDateIsLaterThanEndDate_ThrowsDomainDateExceptionWithSpecifiedMessage()
        {
            Project project = _project;
            DateTime laterDate = _endDate.AddMinutes(1);

            string message = $"You cannot set start date ('{laterDate}') that is later" +
                    $" then current end date ('{project.EndDate}')";

            var result = Assert.Throws<DomainDateException>(() => project.ChangeStartDate(laterDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeStartDate_StartDateIsEqualToEndDate_ThrowsDomainDateExceptionWithSpecifiedMessage()
        {
            Project project = _project;
            DateTime laterDate = _endDate.AddDays(0);

            string message = $"You cannot set start date ('{laterDate}') that is equal to" +
                    $" current end date ('{project.EndDate}')";

            var result = Assert.Throws<DomainDateException>(() => project.ChangeStartDate(laterDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeEndDate_EndDateIsElierThanStartDate_ThrowsDomainDateExceptionWithSpecifiedMessage()
        {
            Project project = _project;
            DateTime elierDate = _startDate.AddDays(-1);

            string message = $"You cannot set end date ('{elierDate}') that is elier" +
                $" then current start date ('{project.StartDate}')";

            var result = Assert.Throws<DomainDateException>(() => project.ChangeEndDate(elierDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void ChangeEndDate_EndDateIsEqualToStartDate_ThrowsDomainDateExceptionWithSpecifiedMessage()
        {
            Project project = _project;
            DateTime elierDate = _startDate.AddDays(0);

            string message = $"You cannot set end date ('{elierDate}') that is equal to" +
                $" current start date ('{project.StartDate}')";

            var result = Assert.Throws<DomainDateException>(() => project.ChangeEndDate(elierDate));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void Ctor_StartDateIsLaterThanEndDate_ThrowsDomainDateExceptionWithSpecifiedMessage()
        {
            Project project;
            string message = $"Start date ('{_endDate}') is later or equal to end date ('{_startDate}')";

            var ex = Assert.Throws<DomainDateException>(() => project = new Project(1, "dfd",
                _endDate,
                _startDate,
                new Priority(1),
                "sss",
                "aaa"));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void AddEmployee_EmployeeShouldKnowAboutTheProjectHeWasAdded()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();

            project.AddEmployee(employee);

            Assert.NotEmpty(project.Employees);
            Assert.Contains(employee, project.Employees);

            Assert.NotEmpty(employee.OnTheseProjectsIsEmployee);
            Assert.Contains(project.Id, employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void AddEmployee_AddEmployeeTwice_EmployeeShouldKnowAboutTheProjectHeWasAddedAndCollectionMustContainOneElement()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();

            project.AddEmployee(employee);
            project.AddEmployee(employee);

            Assert.NotEmpty(project.Employees);
            Assert.Contains(employee, project.Employees);
            
            Assert.NotEmpty(employee.OnTheseProjectsIsEmployee);
            Assert.Contains(project.Id, employee.OnTheseProjectsIsEmployee);

            Assert.Single(employee.OnTheseProjectsIsEmployee);
            Assert.Single(project.Employees);
        }

        [Fact]
        public void RemoveEmployee_BothCollectionsShouldBeEmpty()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);

            project.RemoveEmployee(employee);

            Assert.Empty(project.Employees);
            Assert.Empty(employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeWasNotAddedToTheProjectFirstly_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            string message = "Current emplyee is not work on this project. Add him/her to project first";

            var result = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(employee));
            Assert.Equal(message, result.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsWorking_EmployeesCollectionOfProjectShouldBeEmptyAndManagerNotNullAndManagersCollectionOfEmployeeShouldNotBeEmpty()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);

            project.PromoteEmployeeToManager(employee);

            Assert.Empty(project.Employees);
            Assert.NotNull(project.Manager);
            Assert.Equal(employee, project.Manager);

            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Contains(project.Id, employee.OnTheseProjectsIsManager);
            Assert.Single(employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryingToPromoteAnotherEmployeeButCurrentManagerIsNotNull_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            Employee second = new Employee(2, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
            project.AddEmployee(employee);
            project.AddEmployee(second);
            project.PromoteEmployeeToManager(employee);
            string message = "You must demote or fire current manager first";

            var ex = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(second));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryingToPromoteTheSameEmployee_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            string message = "This emplyee is already manager";

            Employee second = new Employee(1, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
            project.AddEmployee(second);

            var ex = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(second));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ProjectHasNoManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<DomainException>(() => project.DemoteManagerToEmployee(""));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ManagerShouldBeNullAndCollectionOfEmployeesShouldContainsEmployeeAndCollectionOfProjectsManagersIdsShouldContainsProjectsIdAndCollectionOfProjectsEmployeesIdsShouldBeEmpty()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.DemoteManagerToEmployee("reason");

            Assert.Null(project.Manager);
            Assert.Contains(employee, project.Employees);

            Assert.Empty(employee.OnTheseProjectsIsManager);
            Assert.Contains(project.Id, employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void FireManager_ProjectHasNoManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<DomainException>(() => project.FireManager(""));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void FireManager_ManagerShouldBeNullAndAllCollectionsShouldBeEmpty()
        {
            Project project = GetClearProject();
            Employee employee = GetClearEmployee();
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.FireManager("reason");

            Assert.Null(project.Manager);
            Assert.Empty(project.Employees);

            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
        }

        private Project GetClearProject()
        {
            Project result = new Project(1, "Test project",
                _startDate,
                _endDate,
                new Priority(1),
                "CustomerCompany",
                "ContractorCompany");
            return result;
        }

        private Employee GetClearEmployee()
        {
            return new Employee(1, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
        }
    }
}
