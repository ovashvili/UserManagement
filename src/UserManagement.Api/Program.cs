using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Swashbuckle.AspNetCore.Filters;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Mappings;
using UserManagement.Application.Common.Services;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Infrastructure.Extensions;
using UserManagement.Infrastructure.Filters;
using UserManagement.Infrastructure.Logger;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.Context;
using UserManagement.Persistence.Repositories;
using UserManagement.Persistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    builder.Services.AddControllers().AddCustomFilters()
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddDbContext<UserManagementDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementDb"),
            b => b.MigrationsAssembly("UserManagement.Persistence"));
    });

    builder.Services.AddValidatorsFromAssembly(typeof(AuthenticateUserCommand).Assembly);
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AuthenticateUserCommand).Assembly));
    builder.Services.AddSingleton<ILoggerService, LoggerService>();
    builder.Services.AddScoped<IPasswordService, BCryptPasswordService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IRoleManagerService, RoleManagerService>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    builder.Services.Configure<JWTAuthOptions>(options =>
        builder.Configuration.GetSection(JWTAuthOptions.SectionName).Bind(options));

    builder.Services.AddHealthChecks();

    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

    builder.Services.AddAuthorization(opts =>
    {
        opts.AddPolicy("User", policy => { policy.RequireRole("User", "Admin"); });
    });
    
    builder.Services.AddAuthorization(opts =>
    {
        opts.AddPolicy("Admin", policy => { policy.RequireRole("Admin"); });
    });

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(opt =>
    {
        var jwtAuthOptions = builder.Configuration.GetSection(JWTAuthOptions.SectionName).Get<JWTAuthOptions>();

        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions!.Secret)),
            ValidIssuer = jwtAuthOptions.Issuer,
            ValidateIssuer = true,
            ValidAudience = jwtAuthOptions.Audience,
            ValidateAudience = true,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddSwaggerGen(options =>
    {
        options.OperationFilter<SwaggerParameterFilter>();
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);
        options.UseInlineDefinitionsForEnums();
    });

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
    logger.Fatal(ex, "Application failed");
    throw;
}
finally
{
    LogManager.Shutdown();
}