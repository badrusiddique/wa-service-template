using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Wati.Template.Api.Installers.Interfaces;
using Wati.Template.Common.Configurations;
using Wati.Template.Common.Constants;

namespace Wati.Template.Api.Installers;

public class MvcInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        var isAuthEnabled = configuration
            .GetSection("Authentication")
            .Get<AuthenticationConfiguration>()
            .IsEnabled;

        var authentication = services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = InputRequest.DefaultAuthenticationScheme;
                options.DefaultChallengeScheme = InputRequest.DefaultAuthenticationScheme;
                options.DefaultAuthenticateScheme = InputRequest.DefaultAuthenticationScheme;
            });

        if (isAuthEnabled)
        {
            //authentication.AddJwtTokenExtension();
        }
        else
        {
            // if authentication is disabled, allow access to all controllers using proxy-client
            //authentication.AddImpersonatorExtension();
        }

        services
            .AddControllers(options =>
            {
                if (!isAuthEnabled)
                {
                    // If authentication is disabled, allow access to all controllers
                    options.Filters.Add(new AllowAnonymousFilter());
                }
            })
            .AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssemblyContaining<Program>();
                config.ImplicitlyValidateChildProperties = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
    }
}