using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.SharedKernel;
using Sibers.ProjectManagementSystem.SharedKernel.Results;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Commands
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
    {
        private HttpClient _client;
        public CreateEmployeeCommandHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<EmployeeDto>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Post.Create();
                var response = await _client.PostAsJsonAsync(route, request.EmployeeDto, cancellationToken);
                var employee = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<EmployeeDto>(cancellationToken: cancellationToken);
                if (employee == null)
                    return Result<EmployeeDto>.Error("Сотрудник не был создан");
                else
                    return Result.Success<EmployeeDto>(employee);
            }
            catch (Exception e)
            {
                return Result<EmployeeDto>.Error($"Что-то пошло не так. Причина: {e.Message}");
            }
        }
    }
}
