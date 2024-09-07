

using Eshop.DataAccess.Services.Paging;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ISupplierRepository:IRepository<Supplier>
    {
        Task<PagedList<SupplierDTO>> GetSuppliersByPaged(RequestParameter parameter);
        Task UpdateAsync(Supplier supplier);
        Task UpdatePatchAsync(int supplierId,Supplier supplier);
    }
}