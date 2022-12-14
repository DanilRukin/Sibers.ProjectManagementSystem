using Azure;
using Azure.Core;
using Sibers.ProjectManagementSystem.API;
using Sibers.ProjectManagementSystem.API.Controllers;
using Sibers.ProjectManagementSystem.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TaskStatus = Sibers.ProjectManagementSystem.Domain.TaskAgregate.TaskStatus;

namespace Sibers.ProjectManagementSystem.IntegrationTests.ApiControllers
{
    public class TasksControllerTests : IClassFixture<CustomWebApplicationFactory<WebMarker>>
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<WebMarker> _factory;
        private static readonly string _api = "api/tasks";
        public TasksControllerTests(CustomWebApplicationFactory<WebMarker> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Create_NoSuchEmployee_ReturnsNotFoundWithMessage()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = int.MaxValue;
            string message = $"No employee with id: {employeeId}";
            TaskDto task = new TaskDto()
            {
                Name = "some name",
                Priority = 10,
                Description = "comment",
            };

            var response = await _client.PostAsJsonAsync(Post.Create(projectId, employeeId), task);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            IEnumerable<string>? errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task Create_TaskCreated()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee3_WorksOnProject2.Id;

            TaskDto task = new TaskDto()
            {
                Name = "some name",
                Priority = 10,
                Description = "comment",
            };

            var response = await _client.PostAsJsonAsync(Post.Create(projectId, employeeId), task);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(result);
            Assert.NotEqual(task.Id, result.Id);
            Assert.Equal(projectId, result.ProjectId);
            Assert.Equal(employeeId, result.AuthorEmployeeId);

            var project = await _client.GetFromJsonAsync<ProjectDto>($"api/projects/{projectId}/true");
            Assert.NotNull(project);
            Assert.Contains(result.Id, project.TasksIds);
        }

        [Fact]
        public async Task Start_EmployeeIsNotAContractor_ReturnsBadRequestWithMessageAndTaskWasNotStarted()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee1_WorksOnProject1.Id;
            Guid taskId = SeedData.TaskByEmployee1_OnProject1_Employee2_Executor.Id;
            string message = $"Employee (id: {employeeId}) can not start task (id: {taskId}) " +
                    $"because is not a contractor of this task";

            var response = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.Start(taskId, projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var errors = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal(message, errors.First());
        }

        [Fact]
        public async Task Start_TaskStarted()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee2_WorksOnProject1And2.Id;
            Assert.Equal(TaskStatus.ToDo, SeedData.TaskByEmployee1_OnProject1_Employee2_Executor.TaskStatus);
            Guid taskId = SeedData.TaskByEmployee1_OnProject1_Employee2_Executor.Id;

            var response = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.Start(taskId, projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Complete_TaskCompleted()
        {
            int projectId = SeedData.Project1.Id;
            int employeeId = SeedData.Employee2_WorksOnProject1And2.Id;
            Assert.Equal(TaskStatus.ToDo, SeedData.TaskByEmployee1_OnProject1_Employee2_Executor.TaskStatus);
            Guid taskId = SeedData.TaskByEmployee1_OnProject1_Employee2_Executor.Id;

            await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.Start(taskId, projectId, employeeId), UriKind.Relative)
            });

            var response = await _client.SendAsync(new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Put.Complete(taskId, projectId, employeeId), UriKind.Relative)
            });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private static class Post
        {
            internal static string Create(int projectId, int employeeId) 
                => $"{_api}/{nameof(TasksController.Create)}/{projectId}/{employeeId}";
        }

        private static class Put
        {
            internal static string Update() => $"{_api}/{nameof(TasksController.Update)}";
            internal static string Start(Guid taskId, int projectId, int employeeId)
                => $"{_api}/{nameof(TasksController.Start)}/{taskId}/{projectId}/{employeeId}";
            internal static string Complete(Guid taskId, int projectId, int employeeId)
                => $"{_api}/{nameof(TasksController.Complete)}/{taskId}/{projectId}/{employeeId}";
        }
    }
}
