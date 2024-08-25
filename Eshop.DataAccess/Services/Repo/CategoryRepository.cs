using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

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

        public void DeleteAsync(Category entity)
        {
            context.Categories.Remove(entity);
        }

        public void DeleteRangeAsync(IEnumerable<Category> entities)
        {
            context.Categories.RemoveRange(entities);
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable();
                foreach (var item in includes.Split(new[] {","},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            
            var categories = await context.Categories.AsNoTracking().ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<Category>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }
            if (!string.IsNullOrEmpty(search.SortByAsc))
            {
                if (search.SortByAsc.Equals("Id",StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Id);

                }
                if (search.SortByAsc.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Name);
                }
            }
            if (!string.IsNullOrEmpty(search.SortByDesc))
            {
                if (search.SortByDesc.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Id);

                }
                if (search.SortByDesc.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Name);
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

        public async Task<Category> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Category> query = context.Categories.AsNoTracking().AsQueryable();
            if (search.Id != null)
            {
                query = query.Where(x => x.Id == search.Id);
            }
            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.ToLower() == search.Name);
            }
            if (!string.IsNullOrEmpty(search.SortByAsc))
            {
                if (search.SortByAsc.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Id);

                }
                if (search.SortByAsc.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderBy(x => x.Name);
                }
            }
            if (!string.IsNullOrEmpty(search.SortByDesc))
            {
                if (search.SortByDesc.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Id);

                }
                if (search.SortByDesc.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.OrderByDescending(x => x.Name);
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

        public async Task UpdateAsync(int id, CategoryPostDTO dto_category)
        {
            var category = await context.Categories.FindAsync(id);
            category.Name = dto_category.Name;
            category.Description = dto_category.Description;
            category.SupplierId = dto_category.SupplierId;
        }
        public async Task UpdatePatchAsync(int id, Category category)
        {
            var existingCategory = await context.Categories.FindAsync(id);
            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;
                existingCategory.SupplierId = category.SupplierId;
            }
        }
    }
}