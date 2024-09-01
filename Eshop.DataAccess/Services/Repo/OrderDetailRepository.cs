using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class OrderDetailRepository:IOrderDetailRepository
    {
        private readonly AppDbContext context;
        public OrderDetailRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(OrderDetail entity) => await context.OrderDetails.AddAsync(entity);

        public void Delete(OrderDetail entity) =>  context.OrderDetails.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<OrderDetail> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> GetAllAsync(string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderDetail>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}