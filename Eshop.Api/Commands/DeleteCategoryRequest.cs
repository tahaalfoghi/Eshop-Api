using MediatR;

namespace Eshop.Api.Commands
{
    public class DeleteCategoryRequest:IRequest<bool>
    {
        public int CategoryId { get; set; }

        public DeleteCategoryRequest(int CategoryId)
        {
            this.CategoryId = CategoryId;
        }
    }
}
