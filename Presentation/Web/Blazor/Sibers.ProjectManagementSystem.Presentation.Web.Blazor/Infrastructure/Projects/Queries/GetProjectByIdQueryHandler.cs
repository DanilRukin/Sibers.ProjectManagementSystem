using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Projects.Queries
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectDto>>
    {
        private IHttpClientFactory _httpClientFactory;
        private HttpClient _client;

        public GetProjectByIdQueryHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _client = _httpClientFactory.CreateClient();
        }

        public async Task<Result<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.ById(request.ProjectId, request.IncludeAdditionalData);
                ProjectDto? dto = await _client.GetFromJsonAsync<ProjectDto>(route, cancellationToken);
                if (dto == null)
                    return Result<ProjectDto>.Error("Не был получен ответ с сервера.");
                return Result<ProjectDto>.Success(dto);
            }
            catch (Exception e)
            {
                return Result<ProjectDto>.Error(e.ReadErrors());
            }
        }
    }
}
