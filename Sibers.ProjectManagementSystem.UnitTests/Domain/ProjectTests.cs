using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.TaskAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;
using Sibers.ProjectManagementSystem.TestsFakes.Repositories;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class ProjectTests
    {
        private Project _project;
        private DateTime _startDate;
        private DateTime _endDate;

        private int _testProjectId;
        private int _testEmployeeId;

        private FakeGenericRepository<Project> _fakeProjectRepository;
        private FakeGenericRepository<Employee> _fakeEmployeeRepository;
        private ITransferService _transferService;

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
            _testProjectId = 1;
            _testEmployeeId = 1;
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
        public void ChangeTasksContractor_NoSuchEmployeeOnProject_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Task task = project.CreateTask("name", _testEmployeeId);
            int contractorId = _testEmployeeId + 1;
            string message = $"No such employee (id: {contractorId}) on this project. Employee must work on project " +
                $"to become a contractor.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task, contractorId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ContractorIsAuthor_AuthorMustWorkOnProjectButHeWasnt_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Task task = project.CreateTask("name", _testEmployeeId);
            string message = $"No such employee (id: {_testEmployeeId}) on this project. Employee must work on project " +
                $"to become a contractor.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task, _testProjectId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ItIsNotProjectsTask_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            Project project_2 = GetClearProjectWithId(2);
            Task task_2 = project_2.CreateTask("name", _testEmployeeId);
            string message = $"No such task (id: {task_2.Id}) on a project.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task_2, _testEmployeeId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_TaskIsNull_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            string message = $"Task is null.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(null, _testEmployeeId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ContractorChanged()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);

            Assert.Equal(_testEmployeeId, project.Tasks.First(t => t.Id == task.Id).ContractorEmployeeId);
        }

        [Fact]
        public void StartTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.StartTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", _testEmployeeId);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.StartTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_TaskStarted()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            project.StartTask(task);

            Assert.Equal(TaskStatus.InProgress, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.InProgress, task.TaskStatus);
        }

        [Fact]
        public void SuspendTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.SuspendTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", _testEmployeeId);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.SuspendTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_TaskSuspended()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            project.StartTask(task);
            project.SuspendTask(task);

            Assert.Equal(TaskStatus.ToDo, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.ToDo, task.TaskStatus);
        }

        [Fact]
        public void CompleteTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.CompleteTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", _testEmployeeId);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.CompleteTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_TaskCompleted()
        {
            ResetServiceAndRepositories();
            _transferService.AddEmployeeToProject(_testEmployeeId, _testProjectId).Wait();
            Project project = _fakeProjectRepository.Find(new ProjectByIdSpecification(_testProjectId)).Result;
            Task task = project.CreateTask("name", _testEmployeeId);
            project.ChangeTasksContractor(task, _testEmployeeId);
            project.StartTask(task);
            project.CompleteTask(task);

            Assert.Equal(TaskStatus.Completed, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.Completed, task.TaskStatus);
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

        private void ResetServiceAndRepositories()
        {
            _fakeProjectRepository = new FakeGenericRepository<Project>();
            _fakeProjectRepository.AddAsync(GetClearProjectWithId(_testProjectId)).Wait();
            _fakeEmployeeRepository = new FakeGenericRepository<Employee>();
            _fakeEmployeeRepository.AddAsync(GetClearEmployeeWithId(_testEmployeeId)).Wait();
            _transferService = new TransferService(_fakeProjectRepository, _fakeEmployeeRepository);
        }
    }
}
