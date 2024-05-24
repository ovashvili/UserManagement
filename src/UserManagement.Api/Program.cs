using System.Reflection;
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
using Swashbuckle.AspNetCore.Filters;
using UserManagement.Application.Common.Abstractions;
using UserManagement.Application.Common.Mappings;
using UserManagement.Application.Common.Services;
using UserManagement.Application.User.Commands.AuthenticateUser;
using UserManagement.Domain.Common.Abstractions;
using UserManagement.Infrastructure.Extensions;
using UserManagement.Infrastructure.Filters;
using UserManagement.Infrastructure.Options;
using UserManagement.Persistence.Context;
using UserManagement.Persistence.Repositories;
using UserManagement.Persistence.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementDb")));

builder.Services.AddValidatorsFromAssembly(typeof(AuthenticateUserCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(typeof(AuthenticateUserCommand).Assembly));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.Configure<JWTAuthOptions>(options =>
    builder.Configuration.GetSection(JWTAuthOptions.SectionName).Bind(options));

builder.Services.AddHealthChecks();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("User", policy => {
        policy.RequireRole("Basic", "Moderator", "Admin");
    });
});

_ = builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("Moderator", policy => {
        policy.RequireRole("Moderator", "Admin");
    });
});

_ = builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("Admin", policy => {
        policy.RequireRole("Admin");
    });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection(JWTAuthOptions.SectionName).Get<JWTAuthOptions>()!.Secret)),
        ValidIssuer = "jwt",
        ValidateIssuer = true,
        ValidAudience = "jwt-audience",
        ValidateAudience = true,
        RoleClaimType = "Role",
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerParameterFilter>();
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.UseInlineDefinitionsForEnums();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
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