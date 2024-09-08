using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
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

        public void DeleteRangeAsync(IEnumerable<OrderDetail> entities) => context.OrderDetails.AddRange(entities);

        public async Task<PagedList<OrderDetail>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            IQueryable<OrderDetail> query = context.OrderDetails.AsNoTracking().AsQueryable().OrderBy(x => x.Id);
            if(includes is not null)
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return PagedList<OrderDetail>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<IEnumerable<OrderDetail>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetail> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDetail> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}