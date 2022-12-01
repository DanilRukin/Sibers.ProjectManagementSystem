using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using Sibers.ProjectManagementSystem.Domain.Services;
using Sibers.ProjectManagementSystem.TestsFakes.Repositories;
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

        private FakeGenericRepository<Project> _fakeProjectRepository;
        private FakeGenericRepository<Employee> _fakeEmployeeRepository;
        private ITransferService _transferService;

        public TransferServiceTests()
        {
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
            _testProjectId = 1;
            _testEmployeeId = 1;
        }


        [Fact]
        public void AddEmployeeToProject_EmployeeAndProjectShouldKnowAboutEachOther()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testEmployeeId)).Result;

            Assert.NotEmpty(project.Employees);
            Assert.Contains(employee, project.Employees);

            Assert.NotEmpty(employee.OnTheseProjectsIsEmployee);
            Assert.Contains(project, employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void RemoveEmployeeFromProject_BothCollectionsShouldBeEmpty()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();

            _transferService.RemoveEmployeeFromProject(_testEmployeeId, _testProjectId).Wait();

            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testEmployeeId)).Result;

            Assert.Empty(project.Employees);
            Assert.Empty(employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeWasNotAddedToTheProjectFirstly_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            string message = "Current emplyee is not work on this project. Add him/her to project first";

            var result = Assert.Throws<AggregateException>(() =>
                _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait());
            Assert.NotNull(result.InnerException);
            Assert.Equal(result.InnerException.GetType(), typeof(DomainException));
            Assert.Equal(message, result.InnerException.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsWorking_EmployeesCollectionOfProjectShouldBeEmptyAndManagerNotNullAndManagersCollectionOfEmployeeShouldNotBeEmpty()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();

            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testEmployeeId)).Result;

            Assert.NotNull(project.Manager);
            Assert.Equal(employee, project.Manager);

            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Contains(project, employee.OnTheseProjectsIsManager);
            Assert.Single(employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryingToPromoteAnotherEmployeeButCurrentManagerIsNotNull_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            Employee second = new Employee(2, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
            _fakeEmployeeRepository.AddAsync(second).Wait();

            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.AddEmployeeToProject(second.Id, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();

            string message = "You must demote or fire current manager first";

            var ex = Assert.Throws<AggregateException>(() => 
                _transferService.PromoteEmployeeToManager(second.Id, _testProjectId).Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryingToPromoteEmployeeTwice_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();
            string message = "This emplyee is already manager";

            var ex = Assert.Throws<AggregateException>(() 
                => _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ProjectHasNoManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<AggregateException>(() 
                => _transferService.DemoteManagerToEmployee(_testProjectId).Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ManagerShouldBeNullAndCollectionOfEmployeesShouldContainsEmployeeAndCollectionOfProjectsManagersIdsShouldContainsProjectsIdAndCollectionOfProjectsEmployeesIdsShouldBeEmpty()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();

            _transferService.DemoteManagerToEmployee(_testProjectId, "reason").Wait();

            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testProjectId)).Result;
            Assert.Null(project.Manager);
            Assert.Contains(employee, project.Employees);

            Assert.Empty(employee.OnTheseProjectsIsManager);
            Assert.Contains(project, employee.OnTheseProjectsIsEmployee);
        }

        [Fact]
        public void FireManager_ProjectHasNoManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();

            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<AggregateException>(() 
                => _transferService.FireManager(_testProjectId, "reason").Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void FireManager_ManagerShouldBeNullAndAllCollectionsShouldBeEmpty()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();

            _transferService.FireManager(_testProjectId, "reason").Wait();

            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testProjectId)).Result;
            Assert.Null(project.Manager);
            Assert.Empty(project.Employees);

            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_CanNotTransferManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, _testProjectId).Wait();
            int projectId = 2;
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(projectId)).Wait();
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is manager of this project";

            var ex = Assert.Throws<AggregateException>(()
                => _transferService.TransferEmployeeToAnotherProject(_testEmployeeId, _testProjectId, projectId)
                .Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_EmployeeWorksOnBothProjects_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            int projectId = 2;
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(projectId)).Wait();
            _transferService.AddEmployeeToProject(_testEmployeeId, projectId).Wait();
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is already works on project you want to transfer (id: {projectId})";

            var ex = Assert.Throws<AggregateException>(()
                => _transferService.TransferEmployeeToAnotherProject(_testEmployeeId, _testProjectId, projectId)
                .Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_EmployeeWorksOnBothProjectsAndOnFutureProjectHeIsAManager_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            int projectId = 2;
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(projectId)).Wait();
            _transferService.AddEmployeeToProject(_testEmployeeId, projectId).Wait();
            _transferService.PromoteEmployeeToManager(_testEmployeeId, projectId).Wait();
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is already works on project you want to transfer (id: {projectId})";

            var ex = Assert.Throws<AggregateException>(()
                => _transferService.TransferEmployeeToAnotherProject(_testEmployeeId, _testProjectId, projectId)
                .Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_NoSuchEmployeeOnCurrentProject_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            int projectId = 2;
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(projectId)).Wait();
            string message = $"You can not transfer this employee (id: {_testEmployeeId}) from current project" +
                $" (id: {_testProjectId}) because he is not working on it";

            var ex = Assert.Throws<AggregateException>(()
                => _transferService.TransferEmployeeToAnotherProject(_testEmployeeId, _testProjectId, projectId)
                .Wait());
            Assert.NotNull(ex.InnerException);
            Assert.Equal(typeof(DomainException), ex.InnerException.GetType());
            Assert.Equal(message, ex.InnerException.Message);
        }

        [Fact]
        public void TransferEmployeeToAnotherProject_FirstProjectShouldNotKnowAboutTransferredEmployeeButTheSecondShould()
        {
            ResetServiceAndRepositories();
            int projectId = 2;
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(projectId)).Wait();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();

            _transferService.TransferEmployeeToAnotherProject(_testEmployeeId, _testProjectId, projectId).Wait();

            Project firstProject = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Project secondProject = _fakeProjectRepository.Find(new ProjectByIdSpecification(projectId)).Result;
            Employee employee = _fakeEmployeeRepository.Find(new EmployeeByIdSpecification(_testProjectId)).Result;
            Assert.Empty(firstProject.Employees);
            Assert.Contains(employee, secondProject.Employees);
            Assert.Contains(secondProject, employee.OnTheseProjectsIsEmployee);
            Assert.DoesNotContain(firstProject, employee.OnTheseProjectsIsEmployee);
        }

        private void ResetServiceAndRepositories()
        {
            _fakeProjectRepository = new FakeGenericRepository<Project>();
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(_testProjectId)).Wait();
            _fakeEmployeeRepository = new FakeGenericRepository<Employee>();
            _fakeEmployeeRepository.AddAsync(GetClearEmployeeWithId(_testEmployeeId)).Wait();
            _transferService = new TransferService(_fakeProjectRepository, _fakeEmployeeRepository);
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
