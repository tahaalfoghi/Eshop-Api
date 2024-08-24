using eshop.DataAccess.Data;
using Eshop.Models;
using Eshop.Models.Models;
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

        public Task CreateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRangeAsync(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync(string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }
    }
}