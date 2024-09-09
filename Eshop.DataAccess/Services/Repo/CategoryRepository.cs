using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace eshop.DataAccess.Services.Repo
{
    public class CategoryRepository :ICategoryRepository
    {
        private readonly AppDbContext context;
        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Category entity)
        {
            context.Categories.Add(entity);
        }

        public void Delete(Category entity)
        {
            context.Categories.Remove(entity);
        }

        public void DeleteRangeAsync(IEnumerable<Category> entities)
        {
            context.Categories.RemoveRange(entities);
        }

        public async Task<PagedList<Category>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable().OrderBy(x=>x.Name);
                foreach (var item in includes.Split(new[] {","},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Category>.ToPagedList(query,requestParameter.PageNumber,requestParameter.PageSize);
            }
            
            return PagedList<Category>.ToPagedList(context.Categories.OrderBy(x=>x.Name), requestParameter.PageNumber, requestParameter.PageSize);
        }
        public async Task<PagedList<Category>> GetAllByFilterAsync(CategoryRequestParamater requestParameter, string? includes = null)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable();
            if(requestParameter is not null)
            {
                query = query.Filter(requestParameter);
            }
            if (includes is not null)
            {
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Category>.ToPagedList(query,requestParameter.PageNumber,requestParameter.PageSize);
            }
            return PagedList<Category>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<Category> GetByCondition(Expression<Func<Category, bool>> predicate, string? includes= null)
        {
            IQueryable<Category> query = context.Categories.Where(predicate).AsNoTracking().AsQueryable();
            if(includes is not null)
            {
                foreach(var i in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Category> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable();
            if (includes is not null)
            {
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateAsync(Category dto_category)
        {
            var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == dto_category.Id);
            if (existingCategory is not null)
            {
                existingCategory.Name = dto_category.Name;
                existingCategory.Description = dto_category.Description;
                existingCategory.SupplierId = dto_category.SupplierId;
            }
        }

        public async Task UpdatePatchAsync(Category category)
        {
            var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.SupplierId = category.SupplierId;
            }
        }
    }
    public static class CategoryExtensions
    {
        public static IQueryable<Category> Filter(this IQueryable<Category> source, CategoryRequestParamater request)
        {
            return source.Where(x => x.Supplier.CompanyName.Contains(request.SupplierName.Trim().ToLower()));
        }
        public static IQueryable<Category> Search(this IQueryable<Category> source, CategoryRequestParamater request)
        {
            return source.Where(x => x.Name.Contains(request.Name.Trim().ToLower()));
        }
    }
}