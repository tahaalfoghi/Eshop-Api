

using Eshop.Models.Models;

namespace eshop.DataAccess.Services.Repo
{
    public interface ICartRepository:IRepository<Cart>
    {
        Task IncreaseCount(Cart cart, int count);
        Task DecreaseCount(Cart cart, int count);
    }
}