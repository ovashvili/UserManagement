using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Mappings;
using UserManagement.Application.Common.Services;
using UserManagement.Application.User.Commands.AuthenticateUser;

namespace UserManagement.Application.Common.Extensions;

public static class ApplicationDIExtensions
{
    public static IServiceCollection AddApplicationLayerDI(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(AuthenticateUserCommand).Assembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AuthenticateUserCommand).Assembly));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleManagerService, RoleManagerService>();
        builder.Services.AddScoped<IPasswordService, BCryptPasswordService>();
        builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

        return builder.Services;
    }
}