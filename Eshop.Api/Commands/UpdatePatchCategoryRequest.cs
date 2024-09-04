using Eshop.Models.DTOModels;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Eshop.Api.Commands
{
    public class UpdatePatchCategoryRequest:IRequest<bool>
    {
        public int categoryId { get; set; }
        public  JsonPatchDocument<CategoryPostDTO> patch { get; set; }

        public UpdatePatchCategoryRequest(int categoryId,JsonPatchDocument<CategoryPostDTO> patch)
        {
            this.categoryId = categoryId;
            this.patch = patch;
        }
    }
}
