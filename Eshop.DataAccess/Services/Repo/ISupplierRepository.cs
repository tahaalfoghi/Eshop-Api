

using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ISupplierRepository:IRepository<Supplier>
    {
        Task<PagedList<Supplier>> GetAllByFilterAsync(SupplierRequestParamater param, string? include = null);
        Task UpdateAsync(int Id,Supplier supplier);
        Task UpdatePatchAsync(int supplierId,Supplier supplier);
    }
}