using System.IdentityModel.Tokens.Jwt;
using Wati.Template.Api.Middlewares.Extensions;
using Wati.Template.Common.Constants;

namespace Wati.Template.Api.Interceptors.Interfaces;

public class HttpClientInterceptor : IHttpClientInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #region Public Methods

    public async ValueTask<string> GetClientIdAsync()
    {
        var token = _httpContextAccessor.HttpContext.Request.ParseAccessToken();
        return GetClientId(token);
    }

    #endregion


    private string GetClientId(JwtSecurityToken token) => token?.Claims.FirstOrDefault(c =>
        InputRequest.ClientIdClaimKey.Equals(c.Type, StringComparison.OrdinalIgnoreCase))?.Value;
}