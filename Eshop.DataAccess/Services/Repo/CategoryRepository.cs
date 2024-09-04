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

        public void Delete(Category entity)
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
            if(search is not null)
            {
                if(!string.IsNullOrEmpty(search.GlobalFilters))
                {
                    var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if(words.Length == 0)
                    {
                        string filter = words[0];
                        query = query.Where(x => x.Name.Contains(filter)
                                            || x.Supplier.CompanyName.Contains(filter)
                                            || x.SupplierId.ToString().Equals(filter.ToString()));
                    }
                    else
                    {
                        foreach (var word in words)
                        {
                            query = query.Where(x=>x.Name.Contains(word));
                        }
                    }
                }
                if(search.Sort.ToString() is not null)
                {
                    if (search.Sort.ToString().Equals("Asc"))
                        query = query.OrderBy(x => x.Name);
                    if(search.Sort.ToString().Equals("Desc"))
                        query = query.OrderByDescending(x => x.Name);
                    else
                        query =  query.OrderBy(x => x.Name);

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
            if (search is not null)
            {
                if (!string.IsNullOrEmpty(search.GlobalFilters))
                {
                    var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                    {
                        string filter = words[0];
                        query = query.Where(x => x.Name.Contains(filter)
                                     || x.Supplier.CompanyName.Contains(filter)
                                     || x.SupplierId.ToString().Equals(filter.ToString()));
                    }
                    else
                    {
                        foreach (var word in words)
                        {
                            query = query.Where(x => x.Name.Contains(word));
                        }
                    }
                }
                if (search.Sort.ToString() is not null)
                {
                    if (search.Sort.ToString().Equals("Asc",StringComparison.OrdinalIgnoreCase))
                        query = query.OrderBy(x => x.Name);
                    if (search.Sort.ToString().Equals("Desc",StringComparison.OrdinalIgnoreCase))
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
}