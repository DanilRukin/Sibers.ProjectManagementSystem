using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class EmployeeTests
    {
        int _testProjectId, _testEmployeeId;
        private DateTime _startDate;
        private DateTime _endDate;
        public EmployeeTests()
        {
            _testProjectId = 1;
            _testEmployeeId = 1;
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
        }

        [Fact]
        public void CreateTask_ProjectIsNull_ThrowsArgumentNullException()
        {
            Project project = null;
            Employee employee = GetClearEmployeeWithId(1);

            Assert.Throws<ArgumentNullException>(() => employee.CreateTask(project, "name"));
        }

        [Fact]
        public void CreateTask_TaskCreated()
        {
            Project project = GetClearProjectWithId(1);
            Employee employee = GetClearEmployeeWithId(1);
            Task task = employee.CreateTask(project, "name");

            Assert.Contains(task, project.Tasks);
            Assert.Contains(task, employee.CreatedTasks);
            Assert.Empty(employee.ExecutableTasks);
            Assert.Equal(employee.Id, task.AuthorEmployeeId);
        }

        [Fact]
        public void StartTask_TaskIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;

            var ex = Assert.Throws<ArgumentNullException>(() => employee.StartTask(project, task));
        }

        [Fact]
        public void StartTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);  // employee is a contractor of task

            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = employee.CreateTask(project2, "name 2");
            project2.AddEmployee(employee);
            project2.ChangeTasksContractor(task2, employee);  // employee is a contractor of task2
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => employee.StartTask(project, task2));  // project has not task2
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_EmployeeIsNotAContractor_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee author = GetClearEmployeeWithId(1);
            Employee contractor = GetClearEmployeeWithId(2);
            project.AddEmployee(contractor);
            Task task = author.CreateTask(project, "name");
            string message = $"Employee (id: {contractor.Id}) can not start task (id: {task.Id}) " +
                    $"because is not a contractor of this task";

            var ex = Assert.Throws<DomainException>(() => contractor.StartTask(project, task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void StartTask_TaskStarted()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);
            employee.StartTask(project, task);

            Assert.Equal(TaskStatus.InProgress, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.InProgress, task.TaskStatus);
            Assert.Contains(task, employee.ExecutableTasks);
        }

        [Fact]
        public void SuspendTask_TaskIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;

            var ex = Assert.Throws<ArgumentNullException>(() => employee.SuspendTask(project, task));
        }

        [Fact]
        public void SuspendTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);  // employee is a contractor of task

            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = employee.CreateTask(project2, "name 2");
            project2.AddEmployee(employee);
            project2.ChangeTasksContractor(task2, employee);  // employee is a contractor of task2
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => employee.SuspendTask(project, task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_EmployeeIsNotAContractor_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee author = GetClearEmployeeWithId(1);
            Employee contractor = GetClearEmployeeWithId(2);
            project.AddEmployee(contractor);
            Task task = author.CreateTask(project, "name");
            string message = $"Employee (id: {contractor.Id}) can not suspend task (id: {task.Id}) " +
                    $"because is not a contractor of this task";

            var ex = Assert.Throws<DomainException>(() => contractor.SuspendTask(project, task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void SuspendTask_TaskSuspended()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);
            employee.StartTask(project, task);
            employee.SuspendTask(project, task);

            Assert.Equal(TaskStatus.ToDo, project.Tasks.First(t => t.Id == task.Id).TaskStatus);
            Assert.Equal(TaskStatus.ToDo, task.TaskStatus);
        }

        [Fact]
        public void CompleteTask_TaskIsNull_ThrowsArgumentNullException()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = null;

            var ex = Assert.Throws<ArgumentNullException>(() => employee.CompleteTask(project, task));
        }

        [Fact]
        public void CompleteTask_NoSuchTask_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);  // employee is a contractor of task

            Project project2 = GetClearProjectWithId(_testProjectId + 1);
            Task task2 = employee.CreateTask(project2, "name 2");
            project2.AddEmployee(employee);
            project2.ChangeTasksContractor(task2, employee);  // employee is a contractor of task2
            string message = $"No such task (id: {task2.Id}) on project (id: {project.Id}).";

            var ex = Assert.Throws<DomainException>(() => employee.CompleteTask(project, task2));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_EmployeeIsNotAContractor_ThrowsDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            Employee author = GetClearEmployeeWithId(1);
            Employee contractor = GetClearEmployeeWithId(2);
            project.AddEmployee(contractor);
            Task task = author.CreateTask(project, "name");
            string message = $"Employee (id: {contractor.Id}) can not complete task (id: {task.Id}) " +
                    $"because is not a contractor of this task";

            var ex = Assert.Throws<DomainException>(() => contractor.CompleteTask(project, task));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void CompleteTask_TaskCompleted()
        {
            Project project = GetClearProjectWithId(_testProjectId);
            Employee employee = GetClearEmployeeWithId(_testEmployeeId);
            project.AddEmployee(employee);
            Task task = employee.CreateTask(project, "name");
            project.ChangeTasksContractor(task, employee);
            employee.StartTask(project, task);
            employee.CompleteTask(project, task);

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
    }
}
