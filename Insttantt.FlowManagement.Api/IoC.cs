using Insttantt.FlowManagement.Application.Common.Interfaces.Repository;
using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Application.Services;
using Insttantt.FlowManagement.Domain.Pattern;
using Insttantt.FlowManagement.Infrastructure.Repositories;

namespace Insttantt.FlowManagement.Api
{
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services)
        {
            services.AddScoped<IFlowBuilder, FlowBuilder>();   
            services.AddScoped<IFlowService, FlowService>();
            services.AddScoped<IFlowRepository, FlowRepository>();
            services.AddHttpClient();
            services.AddScoped<IFlowExecutionService, FlowExecutionService>();
            services.AddCors();
            return services;
        }
    }
}
