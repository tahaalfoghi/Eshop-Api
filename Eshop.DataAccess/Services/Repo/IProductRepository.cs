

using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IProductRepository:IRepository<Product>
    {
        Task UpdateAsync(int id,Product product);
        Task UpdatePatch(int id,Product product);
    }
}