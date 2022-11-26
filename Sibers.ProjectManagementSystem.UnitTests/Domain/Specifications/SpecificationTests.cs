using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.UnitTests.Domain.Specifications
{
    public class SpecificationTests
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public SpecificationTests()
        {
            _startDate = DateTime.Today;
            _endDate = _startDate.AddDays(1);
        }

        [Fact]
        public void And_IsSatisfiedBy_ProjectByIdAndProjectByName_ValidData_ShouldReturnTrue()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = project.Name;
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool firstResult = projectByIdSpecification.And(projectByNameSpecification).IsSatisfiedBy(project);
            bool secondResult = projectByNameSpecification.And(projectByIdSpecification).IsSatisfiedBy(project);

            Assert.True(firstResult);
            Assert.True(secondResult);
        }

        [Fact]
        public void And_IsSatisfiedBy_ProjectByIdAndProjectByName_InvalidData_ShouldReturnFalse()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = "SomeName";
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool firstResult = projectByIdSpecification.And(projectByNameSpecification).IsSatisfiedBy(project);
            bool secondResult = projectByNameSpecification.And(projectByIdSpecification).IsSatisfiedBy(project);

            Assert.False(firstResult);
            Assert.False(secondResult);
        }

        [Fact]
        public void Or_IsSatisfiedBy_ProjectByIdAndProjectByName_CorrectIdAndIncorrectName_ShouldReturnTrue()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = "SomeName";
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool firstResult = projectByIdSpecification.Or(projectByNameSpecification).IsSatisfiedBy(project);
            bool secondResult = projectByNameSpecification.Or(projectByIdSpecification).IsSatisfiedBy(project);

            Assert.True(firstResult);
            Assert.True(secondResult);
        }

        [Fact]
        public void Or_IsSatisfiedBy_ProjectByIdAndProjectByName_IncorrectIdAndCorrectName_ShouldReturnTrue()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = project.Name;
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id + 1);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool firstResult = projectByIdSpecification.Or(projectByNameSpecification).IsSatisfiedBy(project);
            bool secondResult = projectByNameSpecification.Or(projectByIdSpecification).IsSatisfiedBy(project);

            Assert.True(firstResult);
            Assert.True(secondResult);
        }

        [Fact]
        public void Or_IsSatisfiedBy_ProjectByIdAndProjectByName_IncorrectIdAndIncorrectName_ShouldReturnFalse()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = "SomeName";
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id + 1);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool firstResult = projectByIdSpecification.Or(projectByNameSpecification).IsSatisfiedBy(project);
            bool secondResult = projectByNameSpecification.Or(projectByIdSpecification).IsSatisfiedBy(project);

            Assert.False(firstResult);
            Assert.False(secondResult);
        }

        [Fact]
        public void Not_IsSatisfiedBy_ProjectByIdAndProjectByName_CorrectIdAndCorrectName_ShouldReturnFalse()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = project.Name;
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool result = projectByIdSpecification.Not(projectByNameSpecification).IsSatisfiedBy(project);

            Assert.False(result);
        }

        [Fact]
        public void Not_IsSatisfiedBy_ProjectByIdAndProjectByName_CorrectIdAndIncorrectName_ShouldReturnTrue()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            string name = "SomeName";
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);
            ProjectByNameSpecification projectByNameSpecification = new ProjectByNameSpecification(name);

            bool result = projectByIdSpecification.Not(projectByNameSpecification).IsSatisfiedBy(project);

            Assert.True(result);
        }

        [Fact]
        public void Not_IsSatisfiedBy_ProjectByIdSpecification_CorrectId_ShouldReturnFalse()
        {
            int id = 1;
            Project project = GetClearProjectWithId(id);
            ProjectByIdSpecification projectByIdSpecification = new ProjectByIdSpecification(id);

            bool result = projectByIdSpecification.Not(projectByIdSpecification).IsSatisfiedBy(project);

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
