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

        private List<EmployeeOnProject> _employeesOnProject = new List<EmployeeOnProject>();
        public IReadOnlyCollection<Employee> Employees => _employeesOnProject
            .Select(ep => ep.Employee)
            .ToList()
            .AsReadOnly();

        public Employee? Manager => _employeesOnProject
            ?.Where(ep => ep.Role == EmployeeRoleOnProject.Manager)
            ?.Select(ep => ep.Employee)
            ?.FirstOrDefault();

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

        public void AddEmployee(Employee employee)
        {
            if (employee != null)
            {
                _employeesOnProject ??= new List<EmployeeOnProject>();

                EmployeeOnProject employeeOnProject = new EmployeeOnProject(employee, this, EmployeeRoleOnProject.Employee);
                if (!_employeesOnProject.Contains(employeeOnProject))
                {
                    _employeesOnProject.Add(employeeOnProject);
                    employee.AddProject(employeeOnProject);
                    AddDomainEvent(new EmployeeAddedToTheProjectDomainEvent(employee.Id, Id));
                }
                else
                    throw new DomainException($"Such employee (id: {employee.Id}) is already works on this project");
            }
            else
                throw new ArgumentNullException(nameof(employee));
        }

        public void RemoveEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(Employee));
            EmployeeOnProject employeeOnProject = _employeesOnProject?
                .FirstOrDefault(ep => ep.Employee.Id == employee.Id
                && ep.Project.Id == this.Id);
            if (employeeOnProject != null)
            {
                _employeesOnProject?.Remove(employeeOnProject);
                employee.RemoveProject(employeeOnProject);
                AddDomainEvent(new EmployeeRemovedFromTheProjectDomainEvent(employee.Id, Id));
            }
            else
                throw new DomainException($"No such employee (id: {employee.Id}) on project");
        }

        public void PromoteEmployeeToManager(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            Employee manager = Manager;
            if (employee.Equals(manager))
                throw new DomainException("This emplyee is already manager");
            if (manager != null)
                throw new DomainException("You must demote or fire current manager first");
            _employeesOnProject ??= new List<EmployeeOnProject>();
            EmployeeOnProject employeeOnProject = new EmployeeOnProject(employee, this, EmployeeRoleOnProject.Employee);
            if (!_employeesOnProject.Contains(employeeOnProject))
                throw new DomainException("Current emplyee is not work on this project. Add him/her to project first");
            _employeesOnProject.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Manager);
            employee.PromoteToManagerOnProject(employeeOnProject);
            AddDomainEvent(new EmployeePromotedToManagerDomainEvent(employee.Id, Id));
        }

        public void DemoteManagerToEmployee(string reason)
        {
            var manager = Manager;
            if (manager != null)
            {
                EmployeeOnProject managerOnProject = new EmployeeOnProject(manager, this, EmployeeRoleOnProject.Manager);
                _employeesOnProject.First(ep => ep.Equals(managerOnProject)).ChangeRole(EmployeeRoleOnProject.Employee);
                manager.DemoteToManagerOnProject(managerOnProject);
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(manager.Id, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public void FireManager(string reason)
        {
            var manager = Manager;
            if (manager != null)
            {
                EmployeeOnProject managerOnProject = new EmployeeOnProject(manager, this, EmployeeRoleOnProject.Manager);
                manager.RemoveProject(managerOnProject);
                _employeesOnProject?.Remove(managerOnProject);
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(manager.Id, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
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

        public Task CreateTask(string name, Employee author, Priority priority = null)
        {
            if (priority == null)
                priority = Priority.Default();
            Task task = new Task(Guid.NewGuid(), name, Id, author.Id, priority);
            _tasks = _tasks ?? new List<TaskAgregate.Task>();
            if (!_tasks.Contains(task))
            {
                _tasks.Add(task);
                AddDomainEvent(new TaskCreatedDomainEvent(Id, task, author.Id));
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

        public void ChangeTasksContractor(Task task, Employee employee)
        {
            if (!Employees.Contains(employee))
                throw new DomainException($"No such employee (id: {employee.Id}) on this project." +
                    $" Employee must work on project to become a contractor.");
            if (task != null)
            {
                _tasks ??= new List<TaskAgregate.Task>();
                if (_tasks.Contains(task))
                {
                    _tasks.Find(t => t.Id == task.Id)?.ChangeContractor(employee.Id);
                    task.ChangeContractor(employee.Id);
                    AddDomainEvent(new ContractorChangedDomainEvent(Id, task, employee.Id));
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
