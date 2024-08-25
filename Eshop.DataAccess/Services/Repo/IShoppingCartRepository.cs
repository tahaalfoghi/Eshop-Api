

using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        Task IncreaseCount(ShoppingCart cart, int count);
        Task DecreaseCount(ShoppingCart cart, int count);
    }
}