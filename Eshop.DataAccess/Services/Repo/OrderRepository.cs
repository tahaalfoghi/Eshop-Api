using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
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

        

        public async Task CreateAsync(Order entity)=> await context.Orders.AddAsync(entity);

        public void DeleteAsync(Order entity) => context.Orders.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Order> entities)=> context.Orders.RemoveRange(entities);

        public async Task<IEnumerable<Order>> GetAllAsync(string? includes = null)
        {
            IQueryable<Order> query = context.Orders.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetByIdAsync(int id, string? includes = null)
        {
            if (!string.IsNullOrEmpty(includes))
            {
                IQueryable<Order> query = context.Orders.AsQueryable();
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await context.Orders.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Order> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
        public void ChangeStatus(Order order, OrderStatus status)
        {
            order.Status = status.ToString();
        }
    }
}