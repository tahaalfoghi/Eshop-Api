using eshop.DataAccess.Data;
using eshop.DataAccess.Services.Repo;

namespace eshop.DataAccess.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
      
        public ICategoryRepository CategoryRepository{get;}

        public IProductRepository ProductRepository{get;}

        public ISupplierRepository SupplierRepository{get;}

        public IOrderRepository OrderRepository{get;}

        public IOrderDetailRepository OrderDetailRepository{get;}

        public IShoppingCartRepository ShoppingCartRepository{get;}
        public UnitOfWork(
        ICategoryRepository CategoryRepository, IProductRepository ProductRepository, 
        ISupplierRepository SupplierRepository, IOrderRepository OrderRepository, 
        IOrderDetailRepository OrderDetailRepository, IShoppingCartRepository ShoppingCartRepository, 
        AppDbContext context)
        {
            this.CategoryRepository = CategoryRepository;
            this.ProductRepository = ProductRepository;
            this.SupplierRepository = SupplierRepository;
            this.OrderRepository = OrderRepository;
            this.OrderDetailRepository = OrderDetailRepository;
            this.ShoppingCartRepository = ShoppingCartRepository;
            this.context = context;
        }
        public void Dispose()
        {
            context.Dispose();
        }
        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}