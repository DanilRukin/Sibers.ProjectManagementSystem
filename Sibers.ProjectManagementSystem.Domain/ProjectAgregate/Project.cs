using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events;
using Sibers.ProjectManagementSystem.Domain.TaskAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public class Project : EntityBase<int>, IAgregateRoot
    {
        public string Name { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
        public Priority Priority { get; protected set; }
        public string NameOfTheCustomerCompany { get; protected set; }
        public string NameOfTheContractorCompany { get; protected set; }

        private List<int> _employeesIds = new List<int>();
        public IReadOnlyCollection<int> EmployeesIds => _employeesIds.AsReadOnly();

        public int? ManagerId { get; private set; }

        private List<Task> _tasks = new List<Task>();
        public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();

        protected Project()
        {

        }

        public Project(int id, string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorComapny)
        {
            if (startDate >= endDate)
                throw new DomainDateException($"Start date ('{startDate}') is later or equal to end date ('{endDate}')");
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            ChangeName(name);
            ChangePriority(priority);
            ChangeCustomerCompanyName(nameOfTheCustomerCompany);
            ChangeContractorCompanyName(nameOfTheContractorComapny);
        }

        public Project(int id, string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorCompany, List<int> employeesIds, int? managerId) 
            : this(id, name, startDate, endDate, priority, nameOfTheCustomerCompany, nameOfTheContractorCompany)
        {
            _employeesIds = employeesIds ?? throw new DomainException("Employees is null");
            ManagerId = managerId ?? throw new DomainException("Manager is null");
        }

        internal void PromoteEmployeeToManager(int empId)
        {
            if (empId == default(int))
                throw new DomainException("Employee is incorrect");           
            if (empId.Equals(ManagerId))
                throw new DomainException("This emplyee is already manager");
            if (ManagerId != null)
                throw new DomainException("You must demote or fire current manager first");
            if (!_employeesIds.Contains(empId))
                throw new DomainException("Current emplyee is not work on this project. Add him/her to project first");
            ManagerId = empId;
            RemoveEmployee(empId);
            AddDomainEvent(new EmployeePromotedToManagerDomainEvent((int)ManagerId, Id));
        }

        internal void DemoteManagerToEmployee(string reason)
        {
            if (ManagerId != null)
            {
                int managerId = (int)ManagerId;
                ManagerId = null;
                AddEmployee(managerId);               
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(managerId, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        internal void FireManager(string reason)
        {
            if (ManagerId != null)
            {
                int managerId = (int)ManagerId;
                ManagerId = null;
                AddDomainEvent(new ManagerWasDismissedDomainEvent(managerId, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        internal void AddEmployee(int empId)
        {
            _employeesIds = _employeesIds ?? new List<int>();
            if (ManagerId != null && empId == ManagerId)
                throw new DomainException($"This employee (id: {empId}) is manager. You can not add him to project");
            if (empId != default(int) && !_employeesIds.Contains(empId))
            {
                _employeesIds.Add(empId);
                AddDomainEvent(new EmployeeAddedToTheProjectDomainEvent(empId, Id));
            }               
        }

        internal void RemoveEmployee(int empId)
        {
            if (empId != default(int))
            {
                _employeesIds?.Remove(empId);
                AddDomainEvent(new EmployeeRemovedFromTheProjectDomainEvent(empId, Id));
            }          
        }

        public void ChangeCustomerCompanyName(string name)
        {
            if (name == null)
                throw new DomainException("Customer's company name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Customer's company name cannot be empty or white space");
            NameOfTheCustomerCompany = name;
        }

        public void ChangeContractorCompanyName(string name)
        {
            if (name == null)
                throw new DomainException("Contractor's company name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Contractor's company name cannot be empty or white space");
            NameOfTheContractorCompany = name;
        }

        public void ChangeName(string name)
        {
            if (name == null)
                throw new DomainException("Project's name cannot be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Project's name cannot be empty or white space");
            Name = name;
        }

        public void ChangeStartDate(DateTime startDate)
        {
            if (startDate > EndDate)
                throw new DomainDateException($"You cannot set start date ('{startDate}') that is later" +
                    $" then current end date ('{EndDate}')");
            if (startDate == EndDate)
                throw new DomainDateException($"You cannot set start date ('{startDate}') that is equal to" +
                    $" current end date ('{EndDate}')");
            StartDate = startDate;
        }

        public void ChangeEndDate(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new DomainDateException($"You cannot set end date ('{endDate}') that is elier" +
                $" then current start date ('{StartDate}')");
            if (endDate == StartDate)
                throw new DomainDateException($"You cannot set end date ('{endDate}') that is equal to" +
                $" current start date ('{StartDate}')");
            EndDate = endDate;
        }

        public void ChangePriority(Priority priority)
        {
            if (priority == null)
                throw new DomainException("Priority cannot be null");
            Priority = priority;
        }

        public Task CreateTask(string name, int authorId, Priority priority = null)
        {
            if (priority == null)
                priority = Priority.Default();
            Task task = new Task(Guid.NewGuid(), name, Id, authorId, priority);
            _tasks = _tasks ?? new List<TaskAgregate.Task>();
            if (!_tasks.Contains(task))
            {
                _tasks.Add(task);
                AddDomainEvent(new TaskCreatedDomainEvent(Id, task, authorId));
            }
            return task.Clone();
        }

        public void RemoveTask(Task? task, int employeeId)
        {
            if (task != null)
            {
                _tasks?.Remove(task);
                AddDomainEvent(new TaskRemovedDomainEvent(Id, task, employeeId));
            }
        }

        public void ChangeTasksContractor(Task task, int contractorId)
        {
            _employeesIds ??= new List<int>();
            if (!_employeesIds.Contains(contractorId))
                throw new DomainException($"No such employee (id: {contractorId}) on this project." +
                    $" Employee must work on project to become a contractor.");
            if (task != null)
            {
                _tasks ??= new List<TaskAgregate.Task>();
                if (_tasks.Contains(task))
                {
                    _tasks.Find(t => t.Id == task.Id)?.ChangeContractor(contractorId);
                    task.ChangeContractor(contractorId);
                    AddDomainEvent(new ContractorChangedDomainEvent(Id, task, contractorId));
                }
                else
                    throw new DomainException($"No such task (id: {task.Id}) on a project.");
            }
            else
                throw new DomainException("Task is null.");
        }

        public void StartTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Start();
            task.Start();
        }

        public void SuspendTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Suspend();
            task.Suspend();
        }

        public void CompleteTask(Task task)
        {
            ThrowIfNotValidTask(task);
            _tasks.First(t => t.Id == task.Id).Complete();
            task.Complete();
        }

        private void ThrowIfNotValidTask(Task task)
        {
            if (task == null)
                throw new DomainException("Task is null");
            _tasks ??= new List<TaskAgregate.Task>();
            if (!_tasks.Contains(task))
                throw new DomainException($"No such task (id: {task.Id}) on project (id: {Id}).");
        }
    }
}
