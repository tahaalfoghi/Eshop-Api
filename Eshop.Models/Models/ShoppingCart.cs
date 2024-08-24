namespace Eshop.Models.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = default!;
        public decimal TotalPrice { get; set; }

    }
}