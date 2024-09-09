

using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IProductRepository:IRepository<Product>
    {
        Task<PagedList<Product>> GetAllByFilterAsync(ProductRequestParamater param, string? includes = null);
        Task UpdateAsync(int id,Product product);
        Task UpdatePatch(int id,Product product);
        Task<Product> GetAsync(int id, string? includes = null);
    }
}