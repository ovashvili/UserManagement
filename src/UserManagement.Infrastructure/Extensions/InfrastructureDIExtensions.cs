using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Infrastructure.Logger;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.Repositories;
using UserManagement.Persistence.UnitOfWork;

namespace UserManagement.Infrastructure.Extensions;

public static class InfrastructureDIExtensions
{
    public static IServiceCollection AddInfrastructureLayerDI(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILoggerService, LoggerService>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.Configure<JWTAuthOptions>(options =>
            builder.Configuration.GetSection(JWTAuthOptions.SectionName).Bind(options));
        
        return builder.Services;
    }
}