using MediatR;

namespace Eshop.Api.Commands.Delete
{
    public class DeleteCategoryRequest : IRequest<bool>
    {
        public int CategoryId { get; set; }

        public DeleteCategoryRequest(int CategoryId)
        {
            this.CategoryId = CategoryId;
        }
    }
}
