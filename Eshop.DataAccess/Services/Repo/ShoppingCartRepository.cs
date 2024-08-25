using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class ShoppingCartRepository:IShoppingCartRepository
    {
        private readonly AppDbContext context;
        public ShoppingCartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        

        public void DeleteAsync(ShoppingCart entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeAsync(IEnumerable<ShoppingCart> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShoppingCart>> GetAllAsync(string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ShoppingCart>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseCount(ShoppingCart cart, int count)
        {
            throw new NotImplementedException();
        }

    }
}