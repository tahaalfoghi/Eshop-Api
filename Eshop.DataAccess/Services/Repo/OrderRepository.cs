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
            if(param is not null)
            {
                if (!string.IsNullOrEmpty(param.UserName))
                {
                    query = query.Search(param.UserName);
                }
                if(param.Date !=null)
                {
                    query = query.Filter(param.Date);
                }
            }
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

        public Task<PagedList<Order>> GetAllByFilterAsync(OrderRequestParamater param, string? include = null)
        {
            throw new NotImplementedException();
        }
    }
    public static class OrderExtensions
    {
        public static IQueryable<Order> Filter(this IQueryable<Order> source, DateTime date)
        {
            return source.Where(x => x.OrderDate == date);
        }
        public static IQueryable<Order> Search(this IQueryable<Order> source, string username)
        {
            return source.Where(x => x.ApplicationUser.UserName.Contains(username.Trim().ToLower()));
        }
    }
}