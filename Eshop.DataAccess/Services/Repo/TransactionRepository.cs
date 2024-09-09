﻿using eshop.DataAccess.Data;
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

        public async Task CreateAsync(Transaction entity) => await context.Transactions.AddAsync(entity);

        public void Delete(Transaction entity) => context.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Transaction> entities) => context.RemoveRange(entities);

        public async Task<PagedList<Transaction>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            IQueryable<Transaction> query = context.Transactions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);   
                }
                return PagedList<Transaction>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
            }
            return PagedList<Transaction>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<PagedList<Transaction>> GetAllByFilterAsync(RequestParameter requestParameter, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> GetAsync(Expression<Func<Transaction, bool>> predicate)
        {
            var trans = await context.Transactions.Where(predicate).FirstOrDefaultAsync();
            return trans;
        }

        public Task<Transaction> GetByCondition(Expression<Func<Transaction, bool>> predicate, string? includes = null)
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

        public async Task<Transaction> GetFirstOrDefaultAsync(RequestParameter requestParameter, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public void Update(Transaction trans)
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
