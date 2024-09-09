

using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IOrderDetailRepository:IRepository<OrderDetail>
    {
        Task<PagedList<OrderDetail>> GetAllByFilterAsync(OrderDetailRequestParamater param, string? include = null);
    }
}