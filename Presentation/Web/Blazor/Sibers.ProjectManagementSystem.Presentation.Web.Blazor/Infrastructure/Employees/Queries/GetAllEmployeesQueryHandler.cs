using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private HttpClient _client;
        public GetAllEmployeesQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.All(request.IncludeAdditionalData);
                var result = await _client
                    .GetFromJsonAsync<IEnumerable<EmployeeDto>>(route, cancellationToken)
                    .ConfigureAwait(false);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
