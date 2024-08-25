using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.DTOModels;
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
            if (!string.IsNullOrEmpty(search.SortByAsc))
            {
                if (search.SortByAsc.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Id);

                }
                if (search.SortByAsc.Equals("Categories", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Categories.Count);
                }
            }
            if (!string.IsNullOrEmpty(search.SortByDesc))
            {
                if (search.SortByDesc.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Id);

                }
                if (search.SortByDesc.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Categories.Count);
                }
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

        public async Task<Supplier> GetByIdAsync(int id, string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await context.Suppliers.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Supplier> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {

            IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.CompanyName.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.SortByAsc))
            {
                query = query.OrderBy(x => x.Categories.Count);
            }
            if (!string.IsNullOrWhiteSpace(search.SortByDesc))
            {
                query = query.OrderByDescending(x => x.Categories.Count);
            }
            if (includes is not null)
            {
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync();
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(int Id, Supplier supplier)
        {
            var exists_supplier = await context.Suppliers.FindAsync(Id);
            if (exists_supplier is not null)
            {
                exists_supplier.CompanyName = supplier.CompanyName;
                exists_supplier.ContactName = supplier.ContactName;
                exists_supplier.Address = supplier.Address;
                exists_supplier.Phone = supplier.Phone;
            }
        }
        public async Task UpdatePatchAsync(int Id, Supplier supplier)
        {
            var exists_supplier = await context.Suppliers.FindAsync(Id);
            if (exists_supplier is not null)
            {
                exists_supplier.CompanyName = supplier.CompanyName;
                exists_supplier.ContactName = supplier.ContactName;
                exists_supplier.Address = supplier.Address;
                exists_supplier.Phone = supplier.Phone;
            }
        }
    }
}