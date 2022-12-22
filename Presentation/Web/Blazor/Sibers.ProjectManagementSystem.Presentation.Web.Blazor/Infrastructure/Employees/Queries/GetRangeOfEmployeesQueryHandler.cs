using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetRangeOfEmployeesQueryHandler : IRequestHandler<GetRangeOfEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private HttpClient _client;
        public GetRangeOfEmployeesQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<IEnumerable<EmployeeDto>> Handle(GetRangeOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.Range(request.IncludeAdditionalData);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative),
                    Content = JsonContent.Create(request.EmployeesIds),
                });
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<EmployeeDto>>(cancellationToken: cancellationToken);
                if (result == null)
                    return new List<EmployeeDto>();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
