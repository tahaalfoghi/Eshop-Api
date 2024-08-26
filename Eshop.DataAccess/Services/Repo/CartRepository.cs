using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class CartRepository:ICartRepository
    {
        private readonly AppDbContext context;
        public CartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Cart entity)
        {

        } 

        public void DeleteAsync( Cart entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeAsync(IEnumerable< Cart> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable< Cart>> GetAllAsync(string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable< Cart>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task< Cart> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task< Cart> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseCount(Cart cart, int count)
        {
            throw new NotImplementedException();
        }
        public async Task DecreaseCount(Cart cart, int count)
        {

        }
    }
}