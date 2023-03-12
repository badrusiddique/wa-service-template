namespace Wati.Template.Api.Middlewares.Extensions;

public static class EndpointExtension
{
    public static IApplicationBuilder UseRouteEndpoints(this IApplicationBuilder app) =>
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
}