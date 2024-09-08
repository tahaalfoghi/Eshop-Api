using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
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

        public async Task<PagedList<Product>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            if (!string.IsNullOrEmpty(includes))
            {
                IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();

                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Product>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
            }
            return PagedList<Product>.ToPagedList(context.Products, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<IEnumerable<Product>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if(search is not null)
            {
                if(search.GlobalFilters is not null)
                {
                    var words = search.GlobalFilters.Split(new[] {","},StringSplitOptions.RemoveEmptyEntries);
                    if(words.Length == 0)
                    {
                        var filter = words[0];
                        query = query.Where(x => x.Category.Name.ToLower().Contains(filter.ToLower()));
                    }
                    else
                    {
                        foreach (var word in words)
                        {
                            query = query.Where(x => x.Name.Contains(word) ||
                                          x.Category.Name.Contains(word) ||
                                          x.Price.Equals(word));
                                          
                        }
                    }
                    if (search.Sort.ToString() is not null)
                    {
                        if (search.Sort.ToString().Equals("Asc"))
                            query = query.OrderBy(x => x.Name);
                        if (search.Sort.ToString().Equals("Desc"))
                            query = query.OrderByDescending(x => x.Name);
                        else
                            query = query.OrderBy(x => x.Name);

                    }
                    if (search.Skip > 0)
                    {
                        query = query.Skip(search.Skip);
                    }
                    if (search.Rows > 0)
                    {
                        query = query.Take(search.Rows);
                    }
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
            if (!string.IsNullOrEmpty(search.GlobalFilters))
            {
                var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if(words.Length == 0)
                {
                    string filter = words[0];
                    query = query.Where(x=>x.Name==filter || 
                                        x.Price.ToString() == filter || 
                                        x.Category.Name == filter);
                }
                else
                {
                    foreach(var word in words)
                    {
                        query = query.Where(x => x.Name == word ||
                                        x.Price.ToString() == word ||
                                        x.Category.Name == word);
                    }
                }
                if (!string.IsNullOrEmpty(search.Sort.ToString()))
                {
                    if (search.Sort.ToString().Equals("Asc", StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.OrderBy(x => x.Category.Name);
                    }
                    if (search.Sort.ToString().Equals("Desc", StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.OrderBy(x => x.Category.Name);
                    }
                }
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