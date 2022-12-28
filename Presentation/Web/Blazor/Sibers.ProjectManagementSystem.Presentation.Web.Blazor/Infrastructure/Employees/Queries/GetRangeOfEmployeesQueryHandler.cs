﻿using MediatR;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Dtos;
using Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Extensions;
using Sibers.ProjectManagementSystem.SharedKernel;
using System.Net.Http.Json;

namespace Sibers.ProjectManagementSystem.Presentation.Web.Blazor.Infrastructure.Employees.Queries
{
    public class GetRangeOfEmployeesQueryHandler : IRequestHandler<GetRangeOfEmployeesQuery, Result<IEnumerable<EmployeeDto>>>
    {
        private HttpClient _client;
        public GetRangeOfEmployeesQueryHandler(IHttpClientFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }
        public async Task<Result<IEnumerable<EmployeeDto>>> Handle(GetRangeOfEmployeesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string route = ApiHelper.Get.Range(request.EmployeesIds ,request.IncludeAdditionalData);
                var response = await _client.SendAsync(new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(route, UriKind.Relative),
                });
                var result = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadFromJsonAsync<IEnumerable<EmployeeDto>>(cancellationToken: cancellationToken);
                if (result == null)
                    return Result<IEnumerable<EmployeeDto>>.Error("Не был получен ответ с сервера.");
                return Result<IEnumerable<EmployeeDto>>.Success(result);
            }
            catch (Exception e)
            {
                return Result<IEnumerable<EmployeeDto>>.Error(e.ReadErrors());
            }
        }
    }
}
