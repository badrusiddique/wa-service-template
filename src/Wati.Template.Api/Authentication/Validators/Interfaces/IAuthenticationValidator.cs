using Wati.Template.Common.Configurations;

namespace Wati.Template.Api.Authentication.Validators.Interfaces
{
    public interface IAuthenticationValidator
    {
        ValueTask ValidateAsync(HttpRequest request, AuthenticationConfiguration config);
    }
}
