using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class CartRepository:ICartRepository
    {
        private readonly AppDbContext context;
        public CartRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Cart entity) => await context.Carts.AddAsync(entity);

        public void Delete(Cart entity) => context.Carts.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Cart> entities) => context.Carts.RemoveRange(entities);

        public async Task<IEnumerable<Cart>> GetAllAsync(string? includes = null)
        {
            if (includes is not null)
            {
                IQueryable<Cart> query = context.Carts.AsNoTracking().AsQueryable();
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }

            var carts = await context.Carts.AsNoTracking().ToListAsync();
            return carts;
        }

        public async Task<IEnumerable<Cart>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Cart> query = context.Carts.AsNoTracking().AsQueryable();
            if (search is not null)
            {
                if (search.GlobalFilters is not null)
                {
                    /*var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                    {
                        var filter = words[0];
                        query = query.Where(x => x.Count.ToLower().Contains(filter.ToLower()));
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

                    }*/
                    if (search.Skip >= 1 && search.Rows >= 1)
                    {
                        query = query.Skip(search.Skip).Take(search.Rows);
                    }
                }
            }

            return await query.ToListAsync();
        }
        
        public async Task<Cart> GetByIdAsync(int id, string? includes = null)
        {
            return await context.Carts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Cart> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Cart> query = context.Carts.AsNoTracking().AsQueryable();
            if(search is not null)
            {
                /*if(search.UserId is not null)
                {
                    query = query.Where(x => x.UserId == search.UserId);
                }*/
            }
            if (string.IsNullOrEmpty(includes))
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync(x=>x.UserId == search.GlobalFilters);
        }

        public async Task IncreaseCount(Cart cart, int count)
        {
            cart.Count += count;
        }
        public async Task DecreaseCount(Cart cart, int count)
        {
            cart.Count -= count;
        }
        public async Task<IEnumerable<Cart>> GetUserCart(string userId,string? includes = null)
        {
            if (!string.IsNullOrEmpty(includes))
            {
                IQueryable<Cart> query = context.Carts.AsNoTracking().AsQueryable();
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.ToListAsync();
            }
            var carts = await context.Carts.Where(x => x.UserId == userId).ToListAsync();
            return carts;
        }

        public async Task<Cart> GetCartAsync(Expression<Func<Cart, bool>> predicate, string? includes =null)
        {
            IQueryable<Cart> query = context.Carts.Where(predicate).AsNoTracking().AsQueryable();
            if (query is null)
                throw new Exception($"Cart is null, filter:{predicate}");

            if(includes is not null)
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task Update(Cart cart)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cart>> GetCartsAsync(Expression<Func<Cart, bool>> predicate, string? includes = null)
        {
            return await context.Carts.Where(predicate).ToListAsync();
        }
    }
}