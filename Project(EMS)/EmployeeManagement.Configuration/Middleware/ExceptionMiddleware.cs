using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Configuration.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            NotFoundException ex => BuildResponse(
                HttpStatusCode.NotFound, "404", ex.Message),

            Shared.Exceptions.ValidationException ex => new StandardResponse<object>
            {
                IsSuccess = false,
                StatusCode = "422",
                Message = "Validation failed",
                Errors = ex.Errors
            },

            UnauthorizedException ex => BuildResponse(
                HttpStatusCode.Unauthorized, "401", ex.Message),

            ForbiddenException ex => BuildResponse(
                HttpStatusCode.Forbidden, "403", ex.Message),

            _ => BuildResponse(
                HttpStatusCode.InternalServerError, "500",
                "An unexpected error occurred. Please try again later.")
        };

        context.Response.StatusCode = (int)(exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            Shared.Exceptions.ValidationException => HttpStatusCode.UnprocessableEntity,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ForbiddenException => HttpStatusCode.Forbidden,
            _ => HttpStatusCode.InternalServerError
        });

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }

    private static StandardResponse<object> BuildResponse(
        HttpStatusCode statusCode, string code, string message)
        => new()
        {
            IsSuccess = false,
            StatusCode = code,
            Message = message,
            Errors = new()
        };
}