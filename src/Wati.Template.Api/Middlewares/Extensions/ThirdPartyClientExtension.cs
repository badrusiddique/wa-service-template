namespace Wati.Template.Api.Middlewares.Extensions;

public static class ThirdPartyClientExtension
{
    public static void CreateHttpClients(this IServiceCollection services)
    {
        //var tokenConfig = services
        //    .BuildServiceProvider()
        //    .GetService<IOptions<TokenConfiguration>>()
        //    .Value;

        // register all external api data-source/proxies/context

        services
            .AddHttpClient<IThirdPartyDataContext, ThirdPartyDataContext>()
            .ConfigureHttpClient(c =>
            {
                c.Timeout = TimeSpan.FromMinutes(2.5);
                c.BaseAddress = new Uri("sample-base-url");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                //c.GetTokenAsync(tokenConfig).ConfigureAwait(false).GetAwaiter().GetResult();
            });
    }
}