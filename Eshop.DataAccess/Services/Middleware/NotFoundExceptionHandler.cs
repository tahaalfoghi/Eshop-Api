using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Eshop.DataAccess.Services.Middleware
{
    public sealed class NotFoundExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<NotFoundExceptionHandler> logger;

        public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
        {
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Exception occured {Error Happend in this action method\r\n}", exception.Message);

            var details = new ProblemDetails()
            {
                Detail = $"API Error {exception.Message}",
                Instance = "API",
                Status = (int)HttpStatusCode.NotFound,
                Title = "API ERROR",
                Type = "Not Found"
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
