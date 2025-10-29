using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dotnetfeatures.Services;

/// <summary>
/// Custom implementation of IProblemDetailsService to customize validation error responses
/// </summary>
public class CustomProblemDetailsService : IProblemDetailsService
{
    public ValueTask<bool> TryWriteAsync(ProblemDetailsContext context)
    {
        var httpContext = context.HttpContext;
        var problemDetails = context.ProblemDetails;

        // Customize validation errors (400 Bad Request)
        if (httpContext.Response.StatusCode == 400)
        {
            problemDetails.Title = "Validation Failed";
            problemDetails.Detail = "One or more validation errors occurred. Please check your input and try again.";
            problemDetails.Type = "https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors";

            // Add custom properties
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            problemDetails.Extensions["supportEmail"] = "support@company.com";
            problemDetails.Extensions["documentationUrl"] = "https://docs.company.com/api-validation";

            // Customize error messages to be more user-friendly
            if (problemDetails.Extensions.ContainsKey("errors"))
            {
                var errors = problemDetails.Extensions["errors"] as IDictionary<string, object>;
                if (errors != null)
                {
                    var customErrors = new Dictionary<string, object>();

                    foreach (var error in errors)
                    {
                        var fieldName = error.Key;
                        var errorMessages = error.Value as string[] ?? new[] { error.Value?.ToString() ?? "Invalid value" };

                        // Transform field names to be more user-friendly
                        var friendlyFieldName = fieldName switch
                        {
                            "Name" => "Product Name",
                            "Quantity" => "Product Quantity",
                            "DueDate" => "Due Date",
                            "Title" => "Task Title",
                            _ => fieldName
                        };

                        // Transform error messages to be more helpful
                        var customMessages = errorMessages.Select(msg => msg switch
                  {
                      var m when m.Contains("required") => $"{friendlyFieldName} is required and cannot be empty.",
                      var m when m.Contains("range") => $"{friendlyFieldName} must be between 1 and 1000.",
                      var m when m.Contains("email") => $"Please provide a valid email address for {friendlyFieldName}.",
                      _ => $"{friendlyFieldName}: {msg}"
                  }).ToArray();

                        customErrors[fieldName] = customMessages;
                    }

                    problemDetails.Extensions["errors"] = customErrors;
                }
            }
        }

        // Customize other error types
        else if (httpContext.Response.StatusCode == 404)
        {
            problemDetails.Title = "Resource Not Found";
            problemDetails.Detail = "The requested resource could not be found.";
            problemDetails.Extensions["suggestion"] = "Please check the URL and try again.";
        }

        // Add common extensions for all error responses
        problemDetails.Extensions["apiVersion"] = "v1.0";
        problemDetails.Extensions["serverTime"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");

        return ValueTask.FromResult(false); // Let the default implementation handle the writing
    }

    public async ValueTask WriteAsync(ProblemDetailsContext context)
    {

        await TryWriteAsync(context);
    }
}