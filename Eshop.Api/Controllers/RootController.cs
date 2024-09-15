using Eshop.Models.Links;
using Microsoft.AspNetCore.Mvc;

namespace Eshop.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController:ControllerBase
    {
        private readonly LinkGenerator _linkGenerator;

        public RootController(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }
        [HttpGet(Name = "GetRoot")]
        public async Task<IActionResult> GetRoot()
        {
            var list = new List<Link>
            {
                new Link
                (
                
                   _linkGenerator.GetUriByName(HttpContext,nameof(GetRoot), new { }),
                     "self",
                    "GET"
                ),
                new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"GetSuppliers", new { }),
                     "suppliers",
                    "GET"
                ),
                new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"CreateSupplier", new { }),
                     "create_supplier",
                    "POST"
                ),
                new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"GetCategories", new { }),
                     "categories",
                    "GET"
                ),
                 new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"CreateCategory", new { }),
                     "create_category",
                    "POST"
                ),
                new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"GetProducts", new { }),
                     "products",
                    "GET"
                ),
                new Link
                (

                   _linkGenerator.GetUriByName(HttpContext,"CreateProduct", new { }),
                     "create_product",
                    "POST"
                ),

            };
            return Ok(list);
        }
    }
}
