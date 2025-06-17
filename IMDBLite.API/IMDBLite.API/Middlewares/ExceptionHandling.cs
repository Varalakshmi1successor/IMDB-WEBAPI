using System.Text.Json;
using static IMDBLite.API.Exceptions.CustomExceptions;

namespace IMDBLite.API.Middlewares;

public class ExceptionHandling
{
    private readonly ILogger<ExceptionHandling> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "Invalid JSON");

            if (jsonEx.Message.Contains("Invalid gender"))
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = jsonEx.Message });
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new
                    { error = "Invalid input. Please check your data format." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught");
            context.Response.ContentType = "application/json";
            var statusCode = StatusCodes.Status500InternalServerError;
            var message = "An unexpected error occurred.";

            switch (ex)
            {
                case InvalidNameException:
                case InvalidDOBException:
                case InvalidGenderException:
                case InvalidBioException:
                case InvalidActorException:
                case InvalidProducerException:
                case InvalidMovieException:
                case InvalidGenreException:
                case InvalidReviewException:
                case InvalidAuthException:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = message
            }));
        }
    }
}