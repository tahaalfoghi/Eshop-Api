

using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task UpdateAsync(int id, CategoryPostDTO dto_category);
        Task UpdatePatchAsync(int id, Category category);
    }
}