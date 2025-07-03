using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PayGCompliance.Common;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;

namespace Compliance_Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // continue request pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var response = new ApiResponse<string>();
            var statusCode = HttpStatusCode.InternalServerError;

            // 🔍 Customize by exception type
            switch (ex)
            {
                case ValidationException ve:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<string>.ErrorResponse(ve.Message);
                    break;

                case ArgumentNullException ane:
                    statusCode = HttpStatusCode.BadRequest;
                    response = ApiResponse<string>.ErrorResponse("Missing required data.");
                    break;

                case SqlException sqlEx:
                    statusCode = HttpStatusCode.ServiceUnavailable;
                    response = ApiResponse<string>.ErrorResponse("Database unavailable. Please try again later.");
                    break;

                case UnauthorizedAccessException ua:
                    statusCode = HttpStatusCode.Unauthorized;
                    response = ApiResponse<string>.ErrorResponse("Unauthorized access.");
                    break;

                default:
                    response = ApiResponse<string>.ErrorResponse("Something went wrong. Please try again later.");
                    break;
            }

            _logger.LogError(ex, $"[ErrorHandler] {ex.GetType().Name} occurred while processing {context.Request.Method} {context.Request.Path}");

            context.Response.StatusCode = (int)statusCode;
            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
