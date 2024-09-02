using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Eshop.DataAccess.Services.Middleware
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
            
        }
        public NotFoundException(string? message) : base(message)
        {
        }
        public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        

    }
}
