using Eshop.Models.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Links
{
    public sealed class LinksService : ILinksService
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LinksService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public Link Generate(string endpoint, object? routeValues, string rel, string method)
        {
            var uri = _linkGenerator.GetUriByName(
                _httpContextAccessor.HttpContext,
                endpoint,
                routeValues);

            if (uri == null)
            {
                throw new InvalidOperationException($"Could not resolve endpoint: {endpoint}");
            }

            return new Link(uri, rel, method);
        }

    }
}
