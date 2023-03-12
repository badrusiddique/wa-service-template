namespace Wati.Template.Api.Middlewares.Extensions;

public static class CorsExtension
{
    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
        app.UseCors("CorsPolicy");
}