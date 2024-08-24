using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class OrderRepository:IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Task CreateAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(Order entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeAsync(IEnumerable<Order> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllAsync(string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}