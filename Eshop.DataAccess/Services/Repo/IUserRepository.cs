using eshop.DataAccess.Services.Repo;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.Repo
{
    public interface IUsersRepository:IRepository<ApplicationUser>
    {
        Task<IEnumerable<UserRoleModel>> GetUsersRole();
        Task<ApplicationUser> GetUser(string? Id);
    }
}
