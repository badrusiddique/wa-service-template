using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using Wati.Template.Common.Constants;

namespace Wati.Template.Api.Middlewares.Extensions;

public static class HttpRequestExtension
{
    public static JwtSecurityToken ParseAccessToken(this HttpRequest request) =>
        request.Headers.ContainsKey(InputRequest.AuthorizationHeaderName)
        && AuthenticationHeaderValue.TryParse(request.Headers[InputRequest.AuthorizationHeaderName], out var header)
        && InputRequest.BearerSchemeName.Equals(header.Scheme, StringComparison.OrdinalIgnoreCase)
            ? ParseBearerToken(header.Parameter)
            : null;

    private static JwtSecurityToken ParseBearerToken(string bearerToken)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(bearerToken);
        }
        catch (Exception)
        {
            throw new SecurityTokenException("failed to parse JwtToken");
        }
    }
}