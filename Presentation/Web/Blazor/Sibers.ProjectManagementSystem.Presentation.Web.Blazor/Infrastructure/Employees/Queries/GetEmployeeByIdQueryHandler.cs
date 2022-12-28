using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
    {
        private HttpClient _client;
        public GetEmployeeByIdQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.ById(request.EmployeeId, request.IncludeAdditionalData);
                EmployeeDto? employee = await _client.GetFromJsonAsync<EmployeeDto>(route);
                if (employee == null)
                    return Result<EmployeeDto>.Error("Не был получен ответ с сервера.");
                return Result.Success(employee);
            }
            catch (Exception e)
            {
                return Result<EmployeeDto>.Error(e.ReadErrors());
            }
        }
    }
}
