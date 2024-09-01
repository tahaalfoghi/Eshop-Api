using eshop.DataAccess.Services.Repo;
using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Eshop.DataAccess.Services.Repo
{
    public interface ITransactionRepository:IRepository<Models.Models.Transaction>
    {
        void Update(Models.Models.Transaction trans);
        Task<Models.Models.Transaction> GetAsync(Expression<Func<Models.Models.Transaction,bool>> predicate);
    }
}
