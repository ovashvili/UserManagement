using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UserManagement.Infrastructure.Filters;

namespace UserManagement.Infrastructure.Extensions;

public static class FilterRegistrationExtensions
{
    public static IMvcBuilder AddCustomFilters(this IMvcBuilder builder)
    {
        builder.Services.Configure<MvcOptions>(options =>
        {
            options.Filters.Add<ExceptionHandlingFilterAttribute>();
            options.Filters.Add<CustomValidationFilterAttribute>();
        });

        return builder;
    }
}