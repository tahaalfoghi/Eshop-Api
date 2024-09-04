using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class CreateProductRequest:IRequest<ProductDTO>
    {
        public ProductPostDTO Product { get; set; }

        public CreateProductRequest(ProductPostDTO Product)
        {
            this.Product = Product;
        }
    }
}
