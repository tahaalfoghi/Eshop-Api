using eshop.DataAccess.Data;
using eshop.DataAccess.Services.Repo;
using Eshop.DataAccess.Services.Paging;
using Eshop.DataAccess.Services.Requests;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Repo
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext context;

        public UsersRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(ApplicationUser entity) => await context.ApplicationUsers.AddAsync(entity);

        public void Delete(ApplicationUser entity) => context.ApplicationUsers.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<ApplicationUser> entities) => context.ApplicationUsers.RemoveRange(entities);

        public async Task<PagedList<ApplicationUser>> GetAllAsync(RequestParameter requestParameter,string? includes = null)
        {
            return PagedList<ApplicationUser>.ToPagedList(context.ApplicationUsers,requestParameter.PageNumber,requestParameter.PageSize);
        }

        public async Task<PagedList<ApplicationUser>> GetAllByFilterAsync(RequestParameter requestParameter, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetByCondition(Expression<Func<ApplicationUser, bool>> predicate, string? includes = null)
        {
            IQueryable<ApplicationUser> query = context.ApplicationUsers.AsNoTracking().AsQueryable();
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

        public async Task<ApplicationUser> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }
        public async Task<ApplicationUser> GetUser(string? Id)
        {
            return await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<ApplicationUser> GetUserByUserName(string? userName)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x=>x.UserName.Equals(userName));
            return user;
        }

        public async Task<IEnumerable<UserRoleModel>> GetUsersRole()
        {
            var users = from u in context.ApplicationUsers
                        join ur in context.UserRoles
                        on u.Id equals ur.UserId
                        join r in context.Roles
                        on ur.RoleId equals r.Id
                        select new UserRoleModel
                        {
                            UserName = u.UserName,
                            Email = u.Email,
                            Role = r.Name
                        };
            return users;
                        
        }
    }
   
}
