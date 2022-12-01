using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Domain.EmployeeAgregate
{
    public class Employee : EntityBase<int>, IAgregateRoot
    {
        public PersonalData PersonalData { get; set; }
        public Email Email { get; set; }

        public IReadOnlyCollection<Project> OnTheseProjectsIsEmployee => _employeeOnProjects
            .Where(ep => ep.Role == EmployeeRoleOnProject.Employee)
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        public IReadOnlyCollection<Project> OnTheseProjectsIsManager => _employeeOnProjects
            .Where(ep => ep.Role == EmployeeRoleOnProject.Manager)
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        private List<EmployeeOnProject> _employeeOnProjects = new List<EmployeeOnProject>();
        public IReadOnlyCollection<Project> Projects => _employeeOnProjects
            .Select(ep => ep.Project)
            .ToList()
            .AsReadOnly();

        internal void AddProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();

            if (!_employeeOnProjects.Contains(employeeOnProject))
            {
                _employeeOnProjects.Add(employeeOnProject);
            }
        }

        internal void RemoveProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects?.Remove(employeeOnProject);
        }

        internal void PromoteToManagerOnProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();
            _employeeOnProjects.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Manager);
        }

        internal void DemoteToManagerOnProject(EmployeeOnProject employeeOnProject)
        {
            if (employeeOnProject == null)
                throw new ArgumentNullException(nameof(employeeOnProject));
            _employeeOnProjects ??= new List<EmployeeOnProject>();
            _employeeOnProjects.First(ep => ep.Equals(employeeOnProject)).ChangeRole(EmployeeRoleOnProject.Employee);
        }

        protected Employee() { }

        public Employee(int id, PersonalData personalData, Email email)
        {
            Id = id;
            PersonalData = personalData ?? throw new DomainException("Personal data is null");
            Email = email ?? throw new DomainException("Email is null");
        }

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
