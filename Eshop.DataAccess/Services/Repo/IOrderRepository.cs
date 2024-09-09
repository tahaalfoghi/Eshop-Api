using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task<PagedList<Order>> GetAllByFilterAsync(OrderRequestParamater param, string? include = null);
        void ChangeStatus(Order order, OrderStatus status);
        Task<PagedList<Order>> GetAllByfilterAsync(OrderRequestParamater param, string? includes = null);
    }
}