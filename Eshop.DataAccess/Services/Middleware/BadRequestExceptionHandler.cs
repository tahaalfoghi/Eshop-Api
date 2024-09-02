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
    public class BadRequestException : Exception
    {
        public BadRequestException(string? message) : base(message)
        {
        }
    }
}
