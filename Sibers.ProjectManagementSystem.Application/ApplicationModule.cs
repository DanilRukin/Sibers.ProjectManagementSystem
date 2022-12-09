using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sibers.ProjectManagementSystem.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationWithMediatR(this IServiceCollection services)
        {
            return services.AddMediatR(typeof(ApplicationModule).GetTypeInfo().Assembly);
        }
    }
}
