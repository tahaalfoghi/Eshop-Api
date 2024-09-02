

using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task UpdateAsync(Category dto_category);
        Task UpdatePatchAsync(Category category);
    }
}