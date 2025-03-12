using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface ICartService
    {
        BasketPositionResponseDTO AddToCart(BasketPositionRequestDTO item);
        bool UpdateCartItemQuantity(int productId, int userId, int quantity);
        bool RemoveCartItem(int productId, int userId);
    }
}
