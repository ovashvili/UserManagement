using Asp.Versioning.ApiExplorer;
using NLog;
using NLog.Web;
using UserManagement.Api.Extensions;
using UserManagement.Application.Common.Extensions;
using UserManagement.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    builder.AddApiLayerDI();
    
    builder.AddApplicationLayerDI();

    builder.AddInfrastructureLayerDI();
    
    var app = builder.Build();

    await app.AutomateMigrationAndSeeding();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();

        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });
    }

    app.UseHttpsRedirection();

    app.UseRouting();

    app.UseAuthentication();

    app.UseAuthorization();
    
    app.MapControllers();
    
    app.MapHealthChecks("/APIHealthCheck");

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Application failed.");
    throw;
}
finally
{
    LogManager.Shutdown();
}