using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetRangeOfProjectsQueryHandler : IRequestHandler<GetRangeOfProjectsQuery, IEnumerable<ProjectDto>>
    {
        private HttpClient _client;
        public GetRangeOfProjectsQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetRangeOfProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.Range(request.IncludeAdditionalData);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.ProjectsIds),
                });
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<ProjectDto>>()
                    .ConfigureAwait(false);
                if (result == null)
                    return new List<ProjectDto>();
                else
                    return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
