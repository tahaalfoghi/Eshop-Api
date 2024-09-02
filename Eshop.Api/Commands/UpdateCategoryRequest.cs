using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class UpdateCategoryRequest:IRequest<bool>
    {
       public CategoryPostDTO CategoryDto { get; }

        public UpdateCategoryRequest(CategoryPostDTO CategoryDto)
        {
            this.CategoryDto = CategoryDto;
        }
    }
}
