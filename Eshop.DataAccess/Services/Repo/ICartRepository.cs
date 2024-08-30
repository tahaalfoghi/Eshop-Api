

using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public interface ICartRepository:IRepository<Cart>
    {
        Task IncreaseCount(Cart cart, int count);
        Task DecreaseCount(Cart cart, int count);
        Task<IEnumerable<Cart>> GetUserCart(string userId,string? includes= null);
        Task<Cart> GetCartAsync(Expression<Func<Cart,bool>> predicate,string? includes = null);
        Task<IEnumerable<Cart>> GetCartsAsync(Expression<Func<Cart,bool>> predicate,string? includes = null);
        Task Update(Cart cart);
        
    }
}