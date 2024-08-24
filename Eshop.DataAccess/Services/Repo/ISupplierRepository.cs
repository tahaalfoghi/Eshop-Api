

using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ISupplierRepository:IRepository<Supplier>
    {
        Task UpdateAsync(int Id,Supplier supplier);
        Task UpdatePatchAsync(int Id,Supplier supplier);
    }
}