using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
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

        public async Task<PagedList<Product>> GetAllByFilterAsync(ProductRequestParamater param, string? includes = null)
        {
            IQueryable<Product> query = context.Products.AsNoTracking().AsQueryable();
            if(param is not null)
            {
                if(param.ValidPrice)
                    query = query.Filter(param);
                if (param.CategoryName is not null)
                    query = query.Search(param);
            }
            if(includes is not null)
            {
                foreach (var item in includes.Split(new[] {","},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return PagedList<Product>.ToPagedList(query,param.PageNumber,param.PageSize);
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

        public async Task<Product> GetByCondition(Expression<Func<Product, bool>> predicate, string? includes = null)
        {
            IQueryable<Product> query = context.Products.Where(predicate).AsNoTracking().AsQueryable();
            if (includes is not null)
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.FirstOrDefaultAsync();
        }
    }
    public static class ProductExtensions
    {
        public static IQueryable<Product> Filter(this IQueryable<Product> source, ProductRequestParamater param)
        {
           return source.Where(x => x.Price >= param.Price);
        }
        public static IQueryable<Product> Search(this IQueryable<Product> source, ProductRequestParamater param)
        {
            return source.Where(x => x.Category.Name.ToLower().Contains(param.CategoryName.Trim().ToLower()));
        }
    }
}