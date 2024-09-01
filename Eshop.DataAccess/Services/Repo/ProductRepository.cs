using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext context;

        public ProductRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Product entity) => await context.Products.AddAsync(entity);

        public void Delete(Product entity) => context.Products.Remove(entity);

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
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Asc")
                    query = query.OrderBy(x => x.Name);
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Desc")
                    query = query.OrderByDescending(x => x.Name);
            }
           
            if (!string.IsNullOrEmpty(search.Category))
            {
                query = query.Where(x => x.Category.Name.Equals(search.Category,StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Asc")
                    query = query.OrderBy(x => x.Category.Name);
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Desc")
                    query = query.OrderByDescending(x => x.Category.Name);
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
            if (includes is not null)
            {
                IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();

                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Asc")
                    query = query.OrderBy(x => x.Name);
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Desc")
                    query = query.OrderByDescending(x => x.Name);
            }

            if (!string.IsNullOrEmpty(search.Category))
            {
                query = query.Where(x => x.Category.Name.Equals(search.Category, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Asc")
                    query = query.OrderBy(x => x.Category.Name);
                if (!string.IsNullOrEmpty(search.Sort.ToString()) && search.Sort.ToString() == "Desc")
                    query = query.OrderByDescending(x => x.Category.Name);
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
        public async Task<Product> GetAsync(int id, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if (includes is not null)
            {
                foreach (var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateAsync(int id, Product product)
        {
            var existingProduct = await context.Products.FindAsync(id);
            if (existingProduct is not null)
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
            var existingProduct = await context.Products.FindAsync(id);
            if (existingProduct is not null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Description = product.Description;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ImageUrl = product.ImageUrl;
            }
        }


    }
}