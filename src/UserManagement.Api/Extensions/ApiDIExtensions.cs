using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserManagement.Infrastructure.Extensions;
using UserManagement.Infrastructure.Filters;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.Context;

namespace UserManagement.Api.Extensions;

public static class ApiDIExtensions
{
    public static IServiceCollection AddApiLayerDI(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddCustomFilters()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
        builder.Services.AddDbContext<UserManagementDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementDb"),
                b => b.MigrationsAssembly("UserManagement.Persistence"));
        });
        
        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddHealthChecks();
        
        builder.SetupApiVersioning();
        
        builder.ConfigureAuthorizationRoles();

        builder.SetupJwtAuthorization();
        
        builder.SetupSwaggerDocumentationAndSecurity();
        
        return builder.Services;
    }

    private static void SetupApiVersioning(this WebApplicationBuilder builder)
    {
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
    }

    private static void SetupJwtAuthorization(this WebApplicationBuilder builder)
    {
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
    }
    
    private static void ConfigureAuthorizationRoles(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("User", policy => { policy.RequireRole("User", "Admin"); });
        });

        builder.Services.AddAuthorization(opts =>
        {
            opts.AddPolicy("Admin", policy => { policy.RequireRole("Admin"); });
        });
    }
    
    private static void SetupSwaggerDocumentationAndSecurity(this WebApplicationBuilder builder)
    {
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
    }
}