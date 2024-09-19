using Eshop.Models.DTOModels;
using Eshop.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Services.ModelService
{
    public interface ICartService
    {
        Task AddToCart(CartPostDTO cartPostDTO);
        Task DeleteCartItem(int cartId);
        Task IncreaseCartQuantity(int id, int productId, CartPostDTO cartPostDTO);
        Task DecreaseCartQuantity(int id, int productId, CartPostDTO cartPostDTO);
        Task<IEnumerable<Cart>> Summery();
    }
}
