using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
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

        public void Delete(Order entity) => context.Orders.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Order> entities)=> context.Orders.RemoveRange(entities);

        public async Task<PagedList<Order>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            IQueryable<Order> query = context.Orders.AsNoTracking().AsQueryable().OrderBy(x=>x.Id);
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Order>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
            }
            return PagedList<Order>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<PagedList<Order>> GetAllByFilterAsync(RequestParameter requestParameter , string? includes = null)
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

        public async Task<Order> GetFirstOrDefaultAsync(RequestParameter requestParameter , string? includes = null)
        {
            throw new NotImplementedException();
        }
        public void ChangeStatus(Order order, OrderStatus status)
        {
            order.Status = status.ToString();
        }

        public async Task<PagedList<Order>> GetAllByfilterAsync(OrderRequestParamater param, string? includes = null)
        {
            IQueryable<Order> query = context.Orders.AsNoTracking().AsQueryable();
            if(includes is not null)
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            query = query.Filter(param);
            return PagedList<Order>.ToPagedList(query,param.PageNumber,param.PageSize);
        }

        public async  Task<Order> GetByCondition(Expression<Func<Order, bool>> predicate, string? includes = null)
        {
            IQueryable<Order> query =  context.Orders.Where(predicate).AsNoTracking().AsQueryable();
            if(includes is not null)
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }
    }
    public static class OrderExtensions
    {
        public static IQueryable<Order> Filter(this IQueryable<Order> source, OrderRequestParamater param)
        {
            return source.Where(x => x.ApplicationUser.UserName.ToLower().Contains(param.UserName.Trim().ToLower()));
        }
        public static IQueryable<Order> Search(this IQueryable<Order> source, OrderRequestParamater param)
        {
            return source.Where(x => x.ApplicationUser.UserName.Contains(param.UserName.Trim().ToLower()));
        }
    }
}