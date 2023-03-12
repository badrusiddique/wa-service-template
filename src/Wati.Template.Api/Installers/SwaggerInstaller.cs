using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Common.Configurations;
using Wati.Template.Common.Constants;

namespace Wati.Template.Api.Installers;

public class SwaggerInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var isAuthEnabled = configuration
            .GetSection("Authentication")
            .Get<AuthenticationConfiguration>()
            .IsEnabled;

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "WATI Template Service Web API", Version = "v1" });
            c.DescribeAllEnumsAsStrings();

            // if authentication is enabled, add authentication security definition
            if (isAuthEnabled)
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authentication header using the Bearer scheme",
                    Name = InputRequest.AuthorizationHeaderName,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = InputRequest.BearerSchemeName
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();
            }
            c.OperationFilter<AddDocumentOperationFilter>();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        services.AddSwaggerGenNewtonsoftSupport();
    }
}

// If the action allow anonymous, do not tag a lock to the action in Swagger.
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)
            || context.MethodInfo.DeclaringType == null
            || context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = "Bearer",  Type = ReferenceType.SecurityScheme } },
                        new List<string>()
                    }
                }
            };
    }
}

public class AddDocumentOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.OperationId != "AddDocument")
        {
            return;
        };

        operation.Parameters.Clear();

        var uploadFileMediaType = new OpenApiMediaType
        {
            Schema = new OpenApiSchema
            {
                Type = "object",
                Properties =
                    {
                        ["json"] = new OpenApiSchema { Type = "string" },
                        ["File"] = new OpenApiSchema { Description = "Upload File", Type = "file", Format = "binary" }
                    },
                Required = new HashSet<string> { "File", "json" }
            }
        };

        operation.RequestBody = new OpenApiRequestBody
        {
            Content = { ["multipart/form-data"] = uploadFileMediaType }
        };
    }
}