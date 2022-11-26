using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain.Specifications.ProjectSpecifications
{
    public class ProjectSpecificationsTests
    {
        private DateTime _startDate;
        private DateTime _endDate;
        public ProjectSpecificationsTests()
        {
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
        }

        [Fact]
        public void ProjectByIdSpecification_IsSatisfiedBy_IdsAreSame_ShouldReturnTrue()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            ProjectByIdSpecification specification = new ProjectByIdSpecification(id);

            bool result = specification.IsSatisfiedBy(project);

            Assert.True(result);
        }

        [Fact]
        public void ProjectByIdSpecification_IsSatisfiedBy_IdsAreDifferent_ShouldReturnFalse()
        {
            Project project = GetClearProjectWithId(1);
            ProjectByIdSpecification specification = new ProjectByIdSpecification(2);

            bool result = specification.IsSatisfiedBy(project);

            Assert.False(result);
        }

        [Fact]
        public void ProjectByNameSpecification_NamesAreEqual_ShouldReturnTrue()
        {
            string name = "ProjectName";
            Project project = GetClearProjectWithId(1);
            project.ChangeName(name);
            ProjectByNameSpecification specification = new ProjectByNameSpecification(name);

            bool result = specification.IsSatisfiedBy(project);

            Assert.True(result);
        }

        [Fact]
        public void ProjectByNameSpecification_NamesAreDifferent_ShouldReturnFalse()
        {
            string name = "ProjectName";
            Project project = GetClearProjectWithId(1);
            ProjectByNameSpecification specification = new ProjectByNameSpecification(name);

            bool result = specification.IsSatisfiedBy(project);

            Assert.False(result);
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
    }
}
