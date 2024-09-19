using MediatR;

namespace Eshop.Api.Commands.Delete
{
    public class DeleteProductRequest : IRequest<bool>
    {
        public int ProductId { get; set; }
        public DeleteProductRequest(int ProductId)
        {
            this.ProductId = ProductId;
        }
    }
}
