

using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public interface ICategoryRepository:IRepository<Category>
    {
        Task<PagedList<Category>> GetAllByFilterAsync(CategoryRequestParamater param, string? includes = null);
        Task UpdateAsync(Category dto_category);
        Task UpdatePatchAsync(Category category);
    }
}