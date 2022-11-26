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

        private List<int> _employeesIds = new List<int>();
        public IReadOnlyCollection<int> EmployeesIds => _employeesIds.AsReadOnly();

        public int? ManagerId { get; private set; }

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
    }
}
