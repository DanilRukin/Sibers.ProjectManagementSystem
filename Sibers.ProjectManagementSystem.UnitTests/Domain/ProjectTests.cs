using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.Exceptions;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain
{
    public class ProjectTests
    {
        private Project _project;
        private DateTime _startDate;
        private DateTime _endDate;
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

        private Project GetClearProject()
        {
            Project result = new Project(1, "Test project",
                _startDate,
                _endDate,
                new Priority(1),
                "CustomerCompany",
                "ContractorCompany");
            return result;
        }

        private Employee GetClearEmployee()
        {
            return new Employee(1, new PersonalData("FirstName", "LastName", "Patronymic"),
                new Email("goblin@gmail.com"));
        }
    }
}
