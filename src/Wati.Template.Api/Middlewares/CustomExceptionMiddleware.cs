using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wati.Template.Common.Dtos.Response;
using Wati.Template.Common.Enums;

namespace Wati.Template.Api.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(
        RequestDelegate next,
        ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        ErrorResponse error;
        HttpStatusCode statusCode;

        switch (ex)
        {
            case ArgumentException argEx:
                error = ParseErrorResponse(argEx, ErrorCode.InvalidArg);
                statusCode = HttpStatusCode.BadRequest;
                break;

            case DbUpdateException dbEx:
                error = ParseErrorResponse(dbEx, ErrorCode.DatabaseFailed);
                statusCode = HttpStatusCode.BadRequest;
                break;

            default:
                var defaultEx = new Exception($"the server encountered an internal error or misconfiguration and was unable to complete your request: {ex.Message}", ex);
                error = ParseErrorResponse(defaultEx);
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        _logger.LogError($"API-Request failed: {error}", JsonConvert.SerializeObject(ex));
        await WriteResponseAsync(httpContext, ApiResponse<object>.ParseResponse(statusCode));
    }

    private static async Task WriteResponseAsync(HttpContext httpContext, ApiResponse<object> response)
    {
        httpContext.Response.StatusCode = (int)response.StatusCode;
        httpContext.Response.ContentType = "application/problem+json";
        var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(response));
        await httpContext.Response.Body.WriteAsync(data, 0, data.Length);
    }

    private static ErrorResponse ParseErrorResponse(Exception ex, ErrorCode errorCode = ErrorCode.UnknownError) =>
        new ErrorResponse
        {
            Message = ex.Message,
            Code = ((ErrorCode)(ex.Data["ErrorCode"] ?? errorCode)).ToString()
        };
}