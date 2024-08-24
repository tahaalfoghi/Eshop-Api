using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class ProductRepository:IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Product entity) => await context.Products.AddAsync(entity);

        public void DeleteAsync(Product entity) => context.Products.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Product> entities) => context.Products.RemoveRange(entities);

        public async Task<IEnumerable<Product>> GetAllAsync(string? includes = null)
        {
            if (!string.IsNullOrEmpty(includes))
            {
                IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();

                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            return await context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }
            if (!string.IsNullOrEmpty(search.ASC))
            {
                query = query.OrderBy(x => x.Price);
            }
            if (!string.IsNullOrEmpty(search.DESC))
            {
                query = query.OrderByDescending(x => x.Price);
            }
            if (!string.IsNullOrEmpty(search.Category))
            {
                query = query.Where(x=>x.Category.Name.Contains(search.Category));
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

        public async Task<Product> GetByIdAsync(int id, string? includes = null)
        {
            if (string.IsNullOrEmpty(includes))
            {
                IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();

                foreach (var item in includes.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync();
            }
            return await context.Products.FirstOrDefaultAsync();
        }

        public async Task<Product> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }
            if (!string.IsNullOrEmpty(search.ASC))
            {
                query = query.OrderBy(x => x.Price);
            }
            if (!string.IsNullOrEmpty(search.DESC))
            {
                query = query.OrderByDescending(x => x.Price);
            }
            if (!string.IsNullOrEmpty(search.Category))
            {
                query = query.Where(x => x.Category.Name.Contains(search.Category));
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

        public async Task UpdateAsync(int id, Product product)
        {
            var existingProduct = await context.Products.FindAsync(id);
            if(existingProduct is not null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;

            }
        }

        public async Task UpdatePatch(int id, Product product)
        {
            throw new NotImplementedException();
        }
    }
}