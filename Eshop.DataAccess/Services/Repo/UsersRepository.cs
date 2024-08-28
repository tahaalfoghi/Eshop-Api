using eshop.DataAccess.Data;
using eshop.DataAccess.Services.Repo;
using Eshop.Models;
using Eshop.Models.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void DeleteAsync(ApplicationUser entity) => context.ApplicationUsers.Remove(entity);

        public void DeleteRangeAsync(IEnumerable<ApplicationUser> entities) => context.ApplicationUsers.RemoveRange(entities);

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync(string? includes = null)
        {
            return await context.ApplicationUsers.ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllByFilterAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetByIdAsync(int id, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetFirstOrDefaultAsync(TableSearch search, string? includes = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> GetUser(string? Id)
        {
            return await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == Id);
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
