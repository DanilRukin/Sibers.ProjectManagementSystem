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
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            int contractorId = _testEmployeeId + 1;
            Employee employee1 = GetClearEmployeeWithId(contractorId);
            string message = $"No such employee (id: {contractorId}) on this project. Employee must work on project " +
                $"to become a contractor.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task, employee1));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ContractorIsAuthor_AuthorMustWorkOnProjectButHeWasnt_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            Task task = project.CreateTask("name", employee);
            string message = $"No such employee (id: {_testEmployeeId}) on this project. Employee must work on project " +
                $"to become a contractor.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task, employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ItIsNotProjectsTask_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            Project project_2 = GetClearProjectWithId(2);
            Task task_2 = project_2.CreateTask("name", employee);
            string message = $"No such task (id: {task_2.Id}) on a project.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(task_2, employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_TaskIsNull_ShouldThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            string message = $"Task is null.";

            var ex = Assert.Throws<DomainException>(() => project.ChangeTasksContractor(null, employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeTasksContractor_ContractorChanged()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);

            Assert.Equal(_testEmployeeId, project.Tasks.First(t => t.Id == task.Id).ContractorEmployeeId);
        }

        [Fact]
        public void StartTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.StartTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", employee);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.StartTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_TaskStarted()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            project.StartTask(task);

            Assert.Equal(TaskStatus.InProgress, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.InProgress, task.TaskStatus);
        }

        [Fact]
        public void SuspendTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.SuspendTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", employee);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.SuspendTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_TaskSuspended()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            project.StartTask(task);
            project.SuspendTask(task);

            Assert.Equal(TaskStatus.ToDo, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.ToDo, task.TaskStatus);
        }

        [Fact]
        public void CompleteTask_TaskIsNull_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;
            string message = "Task is null";

            var ex = Assert.Throws<DomainException>(() => project.CompleteTask(task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = project2.CreateTask("name 2", employee);
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => project.CompleteTask(task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_TaskCompleted()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = project.CreateTask("name", employee);
            project.ChangeTasksContractor(task, employee);
            project.StartTask(task);
            project.CompleteTask(task);

            Assert.Equal(TaskStatus.Completed, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.Completed, task.TaskStatus);
        }

        [Fact]
        public void AddEmployee_EmployeeWasAddedEarlier_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            string message = $"Such employee (id: {employee.Id}) is already works on this project";

            var ex = Assert.Throws<DomainException>(() => project.AddEmployee(employee));
            Assert.Equal(message, ex.Message);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);
        }

        [Fact]
        public void AddEmployee_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => project.AddEmployee(employee));
        }

        [Fact]
        public void AddEmployee_EmployeeAdded()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);

            project.AddEmployee(employee);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);

            Assert.Contains(project, employee.Projects);
            Assert.Single(employee.Projects);
        }

        [Fact]
        public void RemoveEmployee_NoSuchEmployee_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            Employee emp = GetClearEmployeeWithId(2);
            string message = $"No such employee (id: {emp.Id}) on project";

            var ex = Assert.Throws<DomainException>(() => project.RemoveEmployee(emp));
            Assert.Equal(message, ex.Message);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Contains(employee, employees);
            Assert.Single(employees);
        }

        [Fact]
        public void RemoveEmployee_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = null;

            var ex = Assert.Throws<ArgumentNullException>(() => project.RemoveEmployee(employee));
        }

        [Fact]
        public void RemoveEmployee_EmployeeRemoved()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);

            project.RemoveEmployee(employee);

            var employees = project.Employees;

            Assert.NotNull(employees);
            Assert.Empty(employees);

            Assert.Empty(employee.Projects);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = null;

            Assert.Throws<ArgumentNullException>(() => project.PromoteEmployeeToManager(employee));
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeeIsNotWorkOnProject_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            string message = "Current emplyee is not work on this project. Add him/her to project first";

            var ex = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_EmployeePromoted()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);

            project.PromoteEmployeeToManager(employee);

            Assert.NotNull(project.Manager);
            Assert.Equal(employee, project.Manager);
            Assert.Contains(project, employee.Projects);
            Assert.Contains(project, employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void PromoteEmployeeToManager_ManagerIsExists_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            string message = "You must demote or fire current manager first";
            Employee employee1 = GetClearEmployeeWithId(2);
            project.AddEmployee(employee1);

            var ex = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(employee1));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void PromoteEmployeeToManager_TryToPromoteTheSameEmployee_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);
            string message = "This emplyee is already manager";

            var ex = Assert.Throws<DomainException>(() => project.PromoteEmployeeToManager(employee));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_NoManager_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<DomainException>(() => project.DemoteManagerToEmployee("reason"));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void DemoteManagerToEmployee_ManagerDemoted()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.DemoteManagerToEmployee("reason");

            Assert.Null(project.Manager);
            Assert.Contains(employee, project.Employees);
            Assert.Contains(project, employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
        }

        [Fact]
        public void FireManager_NoManager_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            string message = "Project has no manager. You have to promote one of the employees to manager.";

            var ex = Assert.Throws<DomainException>(() => project.FireManager("reason"));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void FireManager_ManagerFired()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            project.AddEmployee(employee);
            project.PromoteEmployeeToManager(employee);

            project.FireManager("reason");

            Assert.Null(project.Manager);
            Assert.DoesNotContain(employee, project.Employees);
            Assert.Empty(employee.OnTheseProjectsIsEmployee);
            Assert.Empty(employee.OnTheseProjectsIsManager);
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
