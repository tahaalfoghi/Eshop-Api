using Eshop.Models.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.ModelService
{
    public interface IOrderService
    {
        Task OrderConfirmation(int orderId);
        Task ChangeOrderStatus(int orderId, OrderStatus status);
        Task PlaceOrder(OrderPostDTO orderPostDTO);
    }
    
}
