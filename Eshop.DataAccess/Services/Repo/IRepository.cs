using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.Models;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public interface IRepository<T> where T:class
    {
        Task<PagedList<T>> GetAllAsync(RequestParameter requestParameter , string? includes = null);
        Task<T> GetByIdAsync(int id, string? includes = null);
        Task<T> GetByCondition(Expression<Func<T, bool>> predicate, string? includes = null);
        Task CreateAsync(T entity);
        void Delete(T entity);
        void DeleteRangeAsync(IEnumerable<T> entities);
    }
}