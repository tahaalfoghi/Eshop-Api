using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Middleware
{
    public sealed class BadRequestExceptionHandler:IExceptionHandler
    {
        private readonly ILogger<BadRequestExceptionHandler> logger;

        public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
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
                Status = (int)HttpStatusCode.BadRequest,
                Title = "API ERROR",
                Type = "Bad Request"
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }
    }
}
