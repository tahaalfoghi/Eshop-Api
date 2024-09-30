using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Repo
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext context;

        public TransactionRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Payment entity) => await context.Transactions.AddAsync(entity);

        public void Delete(Payment entity) => context.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Payment> entities) => context.RemoveRange(entities);

        public async Task<PagedList<Payment>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            IQueryable<Payment> query = context.Transactions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);   
                }
                return PagedList<Payment>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
            }
            return PagedList<Payment>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<PagedList<Payment>> GetAllByFilterAsync(RequestParameter requestParameter, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> GetAsync(Expression<Func<Payment, bool>> predicate)
        {
            var trans = await context.Transactions.Where(predicate).FirstOrDefaultAsync();
            return trans;
        }

        public Task<Payment> GetByCondition(Expression<Func<Payment, bool>> predicate, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> GetByIdAsync(int id, string? includes = null)
        {

            IQueryable<Payment> query = context.Transactions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.FirstOrDefaultAsync(x=>x.Id == id);
            }
            return await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Payment> GetFirstOrDefaultAsync(RequestParameter requestParameter, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public void Update(Payment trans)
        {
            var existsTrans = context.Transactions.FirstOrDefault(x => x.Id == trans.Id);
            if(existsTrans is not null)
            {
                existsTrans.UpdateAt = DateTime.Now;
                context.Transactions.Update(existsTrans);
            }
        }
    }
}
