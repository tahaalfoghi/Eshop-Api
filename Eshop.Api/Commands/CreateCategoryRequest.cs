using Eshop.Models.DTOModels;
using MediatR;

namespace Eshop.Api.Commands
{
    public class CreateCategoryRequest:IRequest<CategoryDTO>
    {
        public CategoryPostDTO CategoryPostDto { get; }

        public CreateCategoryRequest(CategoryPostDTO CategoryPostDto)
        {
            this.CategoryPostDto = CategoryPostDto;
        }
    }
}
