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

namespace Sibers.ProjectManagementSystem.FunctionalTests.ApiControllers
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
            };
            string url = "api/projects/create";
            var response = await _client.PostAsJsonAsync(url, requestBody);
            var statusCode = response.StatusCode;
            Assert.True(HttpStatusCode.OK == statusCode);
            var result = await response.Content.ReadFromJsonAsync<ProjectDto>();
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }
    }
}
