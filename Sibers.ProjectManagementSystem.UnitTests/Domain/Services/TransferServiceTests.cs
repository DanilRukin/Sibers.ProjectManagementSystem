using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain.Services
{
    public class TransferServiceTests
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private int _testProjectId;
        private int _testEmployeeId;

        private ITransferService _transferService;

        public TransferServiceTests()
        {
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
            _testProjectId = 1;
            _testEmployeeId = 1;
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_CanNotTransferManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetTransferService();
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            int projectId = 2;
            Project futureProject = GetClearProjectWithId(projectId);
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is manager of this project";

            var ex = Assert.Throws<DomainException>(()
                => _transferService.TransferEmployeeToAnotherProject(employee, project, futureProject));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_EmployeeWorksOnBothProjects_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetTransferService();
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            int projectId = 2;
            Project futureProject = GetClearProjectWithId(projectId);
            futureProject.AddEmployee(employee);
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is already works on project you want to transfer (id: {projectId})";

            var ex = Assert.Throws<DomainException>(()
                => _transferService.TransferEmployeeToAnotherProject(employee, project, futureProject));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_EmployeeWorksOnBothProjectsAndOnFutureProjectHeIsAManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetTransferService();
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            int projectId = 2;
            Project futureProject = GetClearProjectWithId(projectId);
            futureProject.AddEmployee(employee);
            futureProject.PromoteEmployeeToManager(employee);
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is already works on project you want to transfer (id: {projectId})";

            var ex = Assert.Throws<DomainException>(()
                => _transferService.TransferEmployeeToAnotherProject(employee, project, futureProject));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_NoSuchEmployeeOnCurrentProject_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetTransferService();
            int projectId = 2;
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            Project futureProject = GetClearProjectWithId(projectId);
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is not working on it";

            var ex = Assert.Throws<DomainException>(()
                => _transferService.TransferEmployeeToAnotherProject(employee, project, futureProject));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_FirstProjectShouldNotKnowAboutTransferredEmployeeButTheSecondShould()
        {
            ResetTransferService();
            int projectId = 2;
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Project futureProject = GetClearProjectWithId(projectId);

            _transferService.TransferEmployeeToAnotherProject(employee, project, futureProject);

            Assert.Empty(project.Employees);
            Assert.Contains(employee, futureProject.Employees);
            Assert.Contains(futureProject, employee.OnTheseProjectsIsEmployee);
            Assert.DoesNotContain(project, employee.OnTheseProjectsIsEmployee);
        }

        private void ResetTransferService()
        {
            _transferService = new TransferService();
        }

        private Project GetClearProjectWithId(int id)
        {
            Project result = new Project(id, "Test project",
                _startDate,
                _endDate,
                new Priority(1),
                "CustomerCompany",
                "ContractorCompany");
            return result;
        }

        private Employee GetClearEmployeeWithId(int id)
        {
            return new Employee(id, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
        }
    }
}
