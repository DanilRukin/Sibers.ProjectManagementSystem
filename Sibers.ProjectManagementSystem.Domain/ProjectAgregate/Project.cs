using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Events;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.ProjectAgregate
{
    public class Project : EntityBase<int>, IAgregateRoot
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Priority Priority { get; private set; }
        public string NameOfTheCustomerCompany { get; private set; }
        public string NameOfTheContractorCompany { get; private set; }

        private List<Employee> _employees = new List<Employee>();
        public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

        public Employee? Manager { get; private set; }

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

        public Project(int id, string name, DateTime startDate, DateTime endDate, Priority priority, string nameOfTheCustomerCompany, string nameOfTheContractorCompany, List<Employee> employees, Employee? manager) 
            : this(id, name, startDate, endDate, priority, nameOfTheCustomerCompany, nameOfTheContractorCompany)
        {
            _employees = employees ?? throw new DomainException("Employees is null");
            Manager = manager ?? throw new DomainException("Manager is null");
        }

        public void PromoteEmployeeToManager(Employee emp)
        {
            if (emp == null)
                throw new DomainException("Employee is null");
            if (!_employees.Contains(emp))
                throw new DomainException("Current emplyee is not work on this project. Add him/her to project first");
            if (emp.Equals(Manager))
                throw new DomainException("This emplyee is already manager");
            if (Manager != null)
                throw new DomainException("You must demote or fire current manager first");           
            Manager = emp;
            RemoveEmployee(emp);
            Manager.AddOnProjectAsManager(Id);
            AddDomainEvent(new EmployeePromotedToManagerDomainEvent(Manager.Id, Id));
        }

        public void DemoteManagerToEmployee(string reason)
        {
            if (Manager != null)
            {
                int managerId = Manager.Id;
                Manager.RemoveFromProjectAsManager(Id);
                AddEmployee(Manager);
                Manager = null;
                AddDomainEvent(new ManagerDemotedToEmployeeDomainEvent(managerId, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public void FireManager(string reason)
        {
            if (Manager != null)
            {
                int managerId = Manager.Id;
                Manager.RemoveFromProjectAsManager(Id);
                Manager = null;
                AddDomainEvent(new ManagerWasDismissedDomainEvent(managerId, Id, reason));
            }
            else
                throw new DomainException("Project has no manager. You have to promote one of the employees to manager.");
        }

        public void TransferEmployeeToAnotherProject(Employee emp, int newProjectId)
        {
            // TODO: should I use repository or this method is mistake?
        }

        public void AddEmployee(Employee emp)
        {
            _employees = _employees ?? new List<Employee>();
            if (emp != null && !_employees.Contains(emp))
            {
                emp.AddOnProjectAsEmployee(Id);
                _employees.Add(emp);
                AddDomainEvent(new EmployeeAddedToTheProjectDomainEvent(emp.Id, Id));
            }               
        }

        public void RemoveEmployee(Employee emp)
        {
            if (emp != null)
            {
                _employees?.Remove(emp);
                emp.RemoveFromProjectAsEmployee(Id);
                AddDomainEvent(new EmployeeRemovedFromTheProjectDomainEvent(emp.Id, Id));
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
    }
}
