using eshop.DataAccess.Services.Repo;

namespace eshop.DataAccess.Services.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
         ICategoryRepository CategoryRepository { get; }
         IProductRepository ProductRepository { get; }
         ISupplierRepository SupplierRepository { get;}
         IOrderRepository OrderRepository { get; }
         IOrderDetailRepository OrderDetailRepository { get;}
         IShoppingCartRepository ShoppingCartRepository {get;}
         Task CommitAsync();
    }
}