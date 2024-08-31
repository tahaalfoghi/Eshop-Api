using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task CreateAsync(Transaction entity) => await context.Transactions.AddAsync(entity);

        public void DeleteAsync(Transaction entity) => context.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Transaction> entities) => context.RemoveRange(entities);

        public async Task<IEnumerable<Transaction>> GetAllAsync(string? includes = null)
        {
            IQueryable<Transaction> query = context.Transactions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);   
                }
                return await query.ToListAsync();
            }
            return await context.Transactions.ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> GetByIdAsync(int id, string? includes = null)
        {

            IQueryable<Transaction> query = context.Transactions.AsNoTracking().AsQueryable();
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

        public async Task<Transaction> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}
