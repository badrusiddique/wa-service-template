using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Wati.Template.Api.Middlewares;

public class HealthCheckMiddleware
{
    #region Publilc Methods

    public static HealthCheckOptions Ping { get; } = new HealthCheckOptions { ResponseWriter = ParsePingResponse };

    public static HealthCheckOptions Check { get; } = new HealthCheckOptions { ResponseWriter = ParseCheckResponse };

    #endregion

    #region Private Methods

    internal static async Task ParsePingResponse(Microsoft.AspNetCore.Http.HttpContext context, HealthReport report)
    {
        var result = JsonConvert.SerializeObject(
            new
            {
                status = HealthStatus.Healthy.ToString(),
                totalResponseTime = $"{report.TotalDuration.TotalMilliseconds} ms"
            },
            Formatting.Indented,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        context.Response.StatusCode = (int)HttpStatusCode.OK;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }

    internal static async Task ParseCheckResponse(Microsoft.AspNetCore.Http.HttpContext context, HealthReport report)
    {
        var result = JsonConvert.SerializeObject(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => ParseResponseEntry(e.Key, e.Value)),
                totalResponseTime = $"{report.TotalDuration.TotalMilliseconds} ms"
            },
            Formatting.Indented,
            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }

    private static dynamic ParseResponseEntry(string key, HealthReportEntry value) =>
        new
        {
            status = value.Status.ToString(),
            description = key,
            responseTime = $"{value.Duration.TotalMilliseconds} ms"
        };

    #endregion
}