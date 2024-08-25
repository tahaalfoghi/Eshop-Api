

using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IProductRepository:IRepository<Product>
    {
        Task UpdateAsync(int id,Product product);
        Task UpdatePatch(int id,Product product);
        Task<Product> GetAsync(int id, string? includes = null);
    }
}