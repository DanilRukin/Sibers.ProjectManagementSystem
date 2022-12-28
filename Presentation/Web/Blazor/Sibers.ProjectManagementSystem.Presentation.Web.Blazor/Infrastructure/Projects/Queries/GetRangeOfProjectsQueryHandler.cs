using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetRangeOfProjectsQueryHandler : IRequestHandler<GetRangeOfProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        private HttpClient _client;
        public GetRangeOfProjectsQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetRangeOfProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.Range(request.ProjectsIds, request.IncludeAdditionalData);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative),
                });
                if (response == null)
                    return Result<IEnumerable<ProjectDto>>.Error("Не был получен ответ с сервера.");
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<ProjectDto>>()
                    .ConfigureAwait(false);
                if (result == null)
                    return Result<IEnumerable<ProjectDto>>.NotFound("Данные с сервера не были получены.");
                else
                    return Result<IEnumerable<ProjectDto>>.Success(result);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<ProjectDto>>.Error(e.ReadErrors());
            }
        }
    }
}
