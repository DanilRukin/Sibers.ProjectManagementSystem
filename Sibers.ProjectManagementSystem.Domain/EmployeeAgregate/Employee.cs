using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public class Employee : EntityBase<int>, IAgregateRoot
    {
        public PersonalData PersonalData { get; set; }
        public Email Email { get; set; }

        protected List<int> _onTheseProjectsIsEmployee = new List<int>();
        public IReadOnlyCollection<int> OnTheseProjectsIsEmployee => _onTheseProjectsIsEmployee.AsReadOnly();

        protected Employee() { }

        public Employee(int id, PersonalData personalData, Email email)
        {
            Id = id;
            PersonalData = personalData ?? throw new DomainException("Personal data is null");
            Email = email ?? throw new DomainException("Email is null");
        }

        public Employee(int id, PersonalData personalData, Email email, List<int> onTheseProjectsIsEmployee, List<int> onTheseProjectsIsManager)
            : this(id, personalData, email)
        {
            _onTheseProjectsIsEmployee = onTheseProjectsIsEmployee 
                ?? throw new DomainException(nameof(onTheseProjectsIsEmployee) + " is null");
            _onTheseProjectsIsManager = onTheseProjectsIsManager 
                ?? throw new DomainException(nameof(onTheseProjectsIsManager) + " is null");
        }

        internal void AddOnProjectAsEmployee(int projectId)
        {
            _onTheseProjectsIsEmployee = _onTheseProjectsIsEmployee ?? new List<int>();
            if (!_onTheseProjectsIsEmployee.Contains(projectId))
                _onTheseProjectsIsEmployee.Add(projectId);
        }
        internal void RemoveFromProjectAsEmployee(int projectId) => _onTheseProjectsIsEmployee?.Remove(projectId);

        protected List<int> _onTheseProjectsIsManager = new List<int>();
        public IReadOnlyCollection<int> OnTheseProjectsIsManager => _onTheseProjectsIsManager.AsReadOnly();

        internal void AddOnProjectAsManager(int projectId)
        {
            _onTheseProjectsIsManager = _onTheseProjectsIsManager ?? new List<int>();
            if (!_onTheseProjectsIsManager.Contains(projectId))
                _onTheseProjectsIsManager.Add(projectId);
        }

        internal void RemoveFromProjectAsManager(int projectId) => _onTheseProjectsIsManager?.Remove(projectId);

        public void ChangeFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("First name is null or empty");
            PersonalData = new PersonalData(firstName, PersonalData.LastName, PersonalData.Patronymic);
        }

        public void ChangeLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("Last name is null or empty");
            PersonalData = new PersonalData(PersonalData.FirstName, lastName, PersonalData.Patronymic);
        }

        public void ChangePatronymic(string patronymic)
        {
            if (patronymic == null)
                throw new DomainException("Patronymic is null");
            PersonalData = new PersonalData(PersonalData.FirstName, PersonalData.LastName, patronymic);
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainException("Emailis null or empty");
            if (!email.Contains("@"))
                throw new DomainException("Email does not contain '@'");
            Email = new Email(email);
        }
    }
}
