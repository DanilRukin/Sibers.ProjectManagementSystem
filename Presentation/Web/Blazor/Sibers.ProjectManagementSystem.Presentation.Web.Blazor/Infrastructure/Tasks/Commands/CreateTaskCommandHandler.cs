using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Tasks.Commands
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private HttpClient _client;
        public CreateTaskCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Post.Create(request.ProjectId, request.EmployeeId);
                var response = await _client.PostAsJsonAsync<TaskDto>(route, request.Task, cancellationToken);
                TaskDto? result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<TaskDto>(cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
