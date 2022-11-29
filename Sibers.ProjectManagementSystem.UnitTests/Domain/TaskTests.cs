using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class TaskTests
    {
        [Fact]
        public void Start_NoContractor_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            string message = "Task can not be started because it has not contractor employee";

            var ex = Assert.Throws<DomainException>(() => task.Start());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Start_IsAlreadyStarted_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            string message = "Task can not be started because it is already in progress";

            var ex = Assert.Throws<DomainException>(() => task.Start());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Suspend_TaskWasNotStarted_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            string message = "Can not to suspend task because it was not started or was suspended";

            var ex = Assert.Throws<DomainException>(() => task.Suspend());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Suspend_TaskIsCompleted_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            task.Complete();
            string message = "Can not to suspend task because it is completed";

            var ex = Assert.Throws<DomainException>(() => task.Suspend());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Complete_TaskWasNotStarted_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            string message = "Can not complete the task because it was not started";

            var ex = Assert.Throws<DomainException>(() => task.Complete());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void Complete_TaskWasCompletedLater_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);
            task.Start();
            task.Complete();
            string message = "Can not complete the task because it is already completed";

            var ex = Assert.Throws<DomainException>(() => task.Complete());
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_ContractorIsNull_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);            
            string message = "New contractor is null. Use RemoveContractor() method instead.";

            var ex = Assert.Throws<DomainException>(() => task.ChangeContractor(null));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_TheSameContractor_ThrowDomainExceptionWithSpecifiedMessage()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1, contractorId = 2;
            Task task = project.CreateTask("name", authorId);
            task.ChangeContractor(contractorId);
            string message = "This employee is already the contractor of this task";

            var ex = Assert.Throws<DomainException>(() => task.ChangeContractor(contractorId));
            Assert.Equal(message, ex.Message);
        }

        [Fact]
        public void ChangeContractor_ContractorChanged()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1, contractorId = 2;
            Task task = project.CreateTask("name", authorId);

            task.ChangeContractor(contractorId);

            Assert.Equal(contractorId, task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_ContractorIsNotExistsYet_DoNothing()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int? contractorId = task.ContractorEmployeeId;

            task.RemoveContractor();

            Assert.Equal(contractorId, task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_ContractorExists_ContractorBecomeNull()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);

            task.RemoveContractor();

            Assert.Null(task.ContractorEmployeeId);
        }

        [Fact]
        public void RemoveContractor_TasksStatusShouldBeToDo()
        {
            Project project = GetClearProjectWithId(1);
            int authorId = 1;
            Task task = project.CreateTask("name", authorId);
            int contractorId = 2;
            task.ChangeContractor(contractorId);

            task.RemoveContractor();

            Assert.Equal(TaskStatus.ToDo, task.TaskStatus);
        }

        private Project GetClearProjectWithId(int id)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);
            return new Project(id, "ProjectName", startDate, endDate, Priority.Default(),
                "CustomerCompany", "ContractorCompany");
        }
    }
}
