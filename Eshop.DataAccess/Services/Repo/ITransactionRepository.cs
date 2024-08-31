using eshop.DataAccess.Services.Repo;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Eshop.DataAccess.Services.Repo
{
    public interface ITransactionRepository:IRepository<Models.Models.Transaction>
    {
    }
}
