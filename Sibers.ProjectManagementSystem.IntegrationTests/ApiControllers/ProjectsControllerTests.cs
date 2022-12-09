using Sibers.ProjectManagementSystem.API;
using Sibers.ProjectManagementSystem.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sibers.ProjectManagementSystem.IntegrationTests.ApiControllers
{
    public class ProjectsControllerTests : IClassFixture<CustomWebApplicationFactory<WebMarker>>
    {
        private HttpClient _client;
        public ProjectsControllerTests(CustomWebApplicationFactory<WebMarker> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateNewProject()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = startDate.AddDays(1);
            ProjectDto requestBody = new ProjectDto
            {
                Name = "SomeProjectName",
                Priority = 1,
                NameOfTheContractorCompany = "Microsoft",
                NameOfTheCustomerCompany = "Oracle",
                StartDate = startDate,
                EndDate = endDate,
                EmployeesIds = new List<int>(),
                TasksIds = new List<Guid>(),
            };
            string url = "api/projects";
            
            var response = await _client.PostAsJsonAsync<ProjectDto>(url, requestBody);
            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, statusCode);
            var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task GetAll_NotIncludeAdditioanlData_GetAllDataFromDatabase()
        {
            string url = "api/projects";

            var result = await _client.GetFromJsonAsync<IEnumerable<ProjectDto>>(url);

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }

        [Fact]
        public async Task GetAll_IncludeAdditionalData_GetAllFromDatabase()
        {
            string request = "api/projects/all?includeAdditionalData=true";

            var result = await _client.GetFromJsonAsync<IEnumerable<ProjectDto>>(request);

            Assert.NotNull(result);
            ProjectDto project = result.FirstOrDefault(p => p.Id == SeedData.Project1.Id);
            Assert.NotNull(project);
            Assert.NotEmpty(project.TasksIds);
            Assert.NotEmpty(project.EmployeesIds);
        }

        [Fact]
        public async Task GetById_NotIncludeAdditionalData_ReturnProjectDtoWithoutEmployeesAndTasks()
        {
            int id = SeedData.Project1.Id;
            string request = $"api/projects/{id}/false";

            ProjectDto? result = await _client.GetFromJsonAsync<ProjectDto?>(request);

            Assert.NotNull(result);
            Assert.Empty(result.TasksIds);
            Assert.Empty(result.EmployeesIds);
        }
    }
}
