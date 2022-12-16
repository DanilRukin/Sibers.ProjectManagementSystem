using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private IHttpClientFactory _httpClientFactory;
        private HttpClient _client;

        public GetProjectByIdQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _client = _httpClientFactory.CreateClient();
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.ById(request.ProjectId, request.IncludeAdditionalData);
                ProjectDto? dto = await _client.GetFromJsonAsync<ProjectDto>(route, cancellationToken);
                return dto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
