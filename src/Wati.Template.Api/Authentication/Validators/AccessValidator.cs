using Wati.Template.Api.Authentication.Validators.Interfaces;
using Wati.Template.Common.Configurations;

namespace Wati.Template.Api.Authentication.Validators;

public class AccessValidator : IAuthenticationValidator
{
    #region Public Methods

    public async ValueTask ValidateAsync(HttpRequest request, AuthenticationConfiguration config)
    {
        ValidateRequestPath(request.Path);

        await Task.CompletedTask;
    }

    #endregion

    #region Private Methods

    private static void ValidateRequestPath(PathString requestPath)
    {
        var path = requestPath.ToString();

        // only authenticate internal api controllers
        if (!path.StartsWith("/api/"))
        {
            throw new NotSupportedException($"bypassing authentication as the request path does not follow /api/ pattern: {path}");
        }
    }

    #endregion
}