using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;
using System.Text;
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            Console.Write("Reach here...");
            _logger.LogInformation("Request Method: {Method}, Request Path: {Path}", context.Request.Method, context.Request.Path);

            _logger.LogInformation("Request Headers: {Headers}", context.Request.Headers);

            context.Request.EnableBuffering();

            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation("Request Body: {Body}", body);
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the request.");
            await HandleExceptionAsync(context, ex);
        }
    }
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set the response status code and content type
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        // Customize the error message
        var errorMessage = new
        {
            Message = "An unexpected error occurred.",
            Detail = exception.Message // Optionally include exception details (remove in production)
        };
        // Serialize the error message to JSON and write it to the response
        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(errorMessage);
        return context.Response.WriteAsync(jsonResponse);
    }
}