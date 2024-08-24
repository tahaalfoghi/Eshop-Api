using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class SupplierRepository:ISupplierRepository
    {
        private readonly AppDbContext context;
        public SupplierRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Supplier entity) => await context.Suppliers.AddAsync(entity);

        public void DeleteAsync(Supplier entity) => context.Suppliers.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Supplier> entities) => context.Suppliers.RemoveRange(entities);

        public async Task<IEnumerable<Supplier>> GetAllAsync(string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            var suppliers = await context.Suppliers.ToListAsync();
            return suppliers;
        }

        public async Task<IEnumerable<Supplier>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.CompanyName.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.ASC))
            {
                query = query.OrderBy(x => x.Categories.Count);
            }
            if (!string.IsNullOrWhiteSpace(search.DESC))
            {
                query = query.OrderByDescending(x => x.Categories.Count);
            }
            if (includes is not null)
            {
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            return await query.ToListAsync();
        }

        public Task<Supplier> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<Supplier> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}