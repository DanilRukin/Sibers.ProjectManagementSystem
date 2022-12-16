using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private HttpClient _client;
        public GetEmployeeByIdQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.ById(request.EmployeeId, request.IncludeAdditionalData);
                EmployeeDto? result = await _client.GetFromJsonAsync<EmployeeDto>(route);
                if (result == null)
                    return new EmployeeDto();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
