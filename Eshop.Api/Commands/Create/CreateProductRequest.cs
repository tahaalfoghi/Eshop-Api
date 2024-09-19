using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands.Create
{
    public class CreateProductRequest : IRequest<ProductDTO>
    {
        public ProductPostDTO Product { get; set; }

        public CreateProductRequest(ProductPostDTO Product)
        {
            this.Product = Product;
        }
    }
}
