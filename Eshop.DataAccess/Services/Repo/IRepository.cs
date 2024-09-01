using Eshop.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public interface IRepository<T> where T:class
    {
        Task<IEnumerable<T>> GetAllAsync(string? includes = null);
        Task<IEnumerable<T>> GetAllByFilterAsync(TableSearch search, string? includes = null);
        Task<T> GetByIdAsync(int id, string? includes = null);
        Task<T> GetFirstOrDefaultAsync(TableSearch search, string? includes = null);
        Task CreateAsync(T entity);
        void Delete(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
    }
}