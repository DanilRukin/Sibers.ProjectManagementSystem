using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        private HttpClient _httpClient;
        public GetAllProjectsQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _httpClient = factory.CreateClient();
        }
        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.All(request.IncludeAdditionalData);
                var projects = await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDto>>(route, cancellationToken);
                if (projects == null)
                    return new List<ProjectDto>();
                else
                    return projects;
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}
