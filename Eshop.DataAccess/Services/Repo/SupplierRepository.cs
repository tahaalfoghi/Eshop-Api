using AutoMapper;
using eshop.DataAccess.Data;
using Eshop.DataAccess.Services.Paging;
using Eshop.Models;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
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

        public async Task<IEnumerable<Supplier>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
            if (search is not null)
            {
                if (!string.IsNullOrEmpty(search.GlobalFilters))
                {
                    var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                    {
                        string filter = words[0];
                        query = query.Where(x => x.CompanyName.Contains(filter)
                                            || x.ContactName.Contains(filter)
                                            || x.Country.ToString().Equals(filter.ToString()));
                    }
                    else
                    {
                        foreach (var word in words)
                        {
                            query = query.Where(x => x.CompanyName.Contains(word));
                        }
                    }
                }
                if (search.Sort.ToString() is not null)
                {
                    if (search.Sort.ToString().Equals("Asc"))
                        query = query.OrderBy(x => x.CompanyName);
                    if (search.Sort.ToString().Equals("Desc"))
                        query = query.OrderByDescending(x => x.CompanyName);
                    else
                        query = query.OrderBy(x => x.CompanyName);

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

        public async Task<Supplier> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {

            IQueryable<Supplier> query = context.Suppliers.AsNoTracking().AsQueryable();
            query = query.Where(x => x.CompanyName.Contains(search.GlobalFilters));
            if (search is not null)
            {
                if (!string.IsNullOrEmpty(search.GlobalFilters))
                {
                    var words = search.GlobalFilters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 0)
                    {
                        string filter = words[0];
                        query = query.Where(x => x.CompanyName.Contains(filter)
                                            || x.ContactName.Contains(filter)
                                            || x.Country.Contains(filter));
                    }
                    else
                    {
                        foreach (var word in words)
                        {
                            query = query.Where(x => x.CompanyName.Contains(word));
                        }
                    }
                }
                if (search.Sort.ToString() is not null)
                {
                    if (search.Sort.ToString().Equals("Asc"))
                        query = query.OrderBy(x => x.CompanyName);
                    if (search.Sort.ToString().Equals("Desc"))
                        query = query.OrderByDescending(x => x.Id);
                    else
                        query = query.OrderBy(x => x.Id);

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
        public async Task<PagedList<SupplierDTO>> GetSuppliersByPaged(RequestParameter parameter)
        {
            var suppliers = await context.Suppliers.OrderBy(x=>x.Id).ToListAsync();
            var dto = mapper.Map<List<SupplierDTO>>(suppliers);
            return PagedList<SupplierDTO>.ToPagedList(dto, parameter.PageNumber, parameter.PageSize);
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
}