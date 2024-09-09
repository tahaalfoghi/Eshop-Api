using AutoMapper;
using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eshop.DataAccess.Services.Repo
{
    public class SupplierRepository:ISupplierRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public SupplierRepository(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task CreateAsync(Supplier entity) => await context.Suppliers.AddAsync(entity);

        public void Delete(Supplier entity) => context.Suppliers.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<Supplier> entities) => context.Suppliers.RemoveRange(entities);

        public async Task<PagedList<Supplier>> GetAllAsync(RequestParameter requestParameter ,string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Supplier>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
            }
            var suppliers = await context.Suppliers.ToListAsync();
            return PagedList<Supplier>.ToPagedList(suppliers, requestParameter.PageNumber, requestParameter.PageSize);

        }

        public async Task<PagedList<Supplier>> GetAllByFilterAsync(SupplierRequestParamater requestParameter, string? includes = null)
        {
            IQueryable<Supplier> query = context.Suppliers.AsNoTracking()
                                                          .Serach(requestParameter.CompanyName)
                                                          .Filter(requestParameter.Country)
                                                          .AsQueryable();
            if (includes is not null)
            {
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return PagedList<Supplier>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);

            }
            return PagedList<Supplier>.ToPagedList(query, requestParameter.PageNumber, requestParameter.PageSize);
        }

        public async Task<Supplier> GetByCondition(Expression<Func<Supplier, bool>> predicate, string? includes = null)
        {
            IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
            query = query.Where(predicate);
            if(includes is not null)
            {
                foreach(var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Supplier> GetByIdAsync(int id, string? includes = null)
        {
            if(includes is not null)
            {
                IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
                foreach (var item in includes.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
                return await query.FirstOrDefaultAsync(x => x.Id == id);
            }
            return await context.Suppliers.FirstOrDefaultAsync(x=>x.Id == id);
        }

        

        public async Task UpdateAsync(Supplier supplier)
        {
            var exists_supplier = await context.Suppliers.FirstOrDefaultAsync(x => x.Id == supplier.Id);
            if (exists_supplier is not null)
            {
                exists_supplier.CompanyName = supplier.CompanyName;
                exists_supplier.ContactName = supplier.ContactName;
                exists_supplier.Address = supplier.Address;
                exists_supplier.Phone = supplier.Phone;
            }
        }
        public async Task UpdatePatchAsync(int supplierId, Supplier supplier)
        {
            var exists_supplier = await context.Suppliers.FirstOrDefaultAsync(x=>x.Id == supplierId);
            if (exists_supplier is not null)
            {
                exists_supplier.CompanyName = supplier.CompanyName;
                exists_supplier.ContactName = supplier.ContactName;
                exists_supplier.Address = supplier.Address;
                exists_supplier.Phone = supplier.Phone;
            }
        }
    }
    public static class SupplierExtensions
    {
        public static IQueryable<Supplier> Serach(this IQueryable<Supplier> source, string search)
        {
            if(string.IsNullOrEmpty(search))
                return source;

            return source.Where(x => x.CompanyName.Contains(search.Trim().ToLower()));
        }
        public static IQueryable<Supplier> Filter(this IQueryable<Supplier> source, string filter)
        {
            if(string.IsNullOrEmpty(filter))
                return source;

            return source.Where(x => x.Country.Contains(filter.Trim().ToLower()));
        }
    }
}