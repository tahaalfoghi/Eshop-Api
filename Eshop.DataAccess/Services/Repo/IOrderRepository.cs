

using Eshop.Models.DTOModels;
using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IOrderRepository:IRepository<Order>
    {
         public void ChangeStatus(Order order, OrderStatus status);
    }
}