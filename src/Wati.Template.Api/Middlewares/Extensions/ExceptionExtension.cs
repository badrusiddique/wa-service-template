namespace Wati.Template.Api.Middlewares.Extensions;

public static class ExceptionExtension
{
    public static IApplicationBuilder UseCustomException(this IApplicationBuilder app) =>
        app.UseMiddleware<CustomExceptionMiddleware>();
}