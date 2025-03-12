using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IBasketService
    {
        Task<BasketPositionResponseDTO> AddToBasket(BasketPositionRequestDTO item);
        Task<bool> UpdateBasketItemQuantity(int productId, int userId, int quantity);
        Task<bool> RemoveBasketItem(int productId, int userId);
    }
}
