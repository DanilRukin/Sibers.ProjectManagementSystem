using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sibers.ProjectManagementSystem.Application.Dtos;
using Sibers.ProjectManagementSystem.Application.Services.Interfaces;
using Sibers.ProjectManagementSystem.Application.Services.Mappers;
using Sibers.ProjectManagementSystem.Domain.EmployeeAgregate;
using Sibers.ProjectManagementSystem.Domain.ProjectAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Task = Sibers.ProjectManagementSystem.Domain.TaskAgregate.Task;

namespace Sibers.ProjectManagementSystem.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationWithMediatR(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(ApplicationModule).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddApplicationMappers(this IServiceCollection services)
        {
            services.AddScoped<IMapper<Project, ProjectDto>, ProjectToProjectDtoMapper>();
            services.AddScoped<IMapper<Employee, EmployeeDto>, EmployeeToEmployeeDtoMapper>();
            services.AddScoped<IMapper<Task, TaskDto>, TaskToTaskDtoMapper>();
            return services;
        }
    }
}
