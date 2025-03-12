using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_EF.Repositories
{
    public class BasketService : IBasketService
    {
        private readonly WebstoreContext _context;

        public BasketService(WebstoreContext context)
        {
            _context = context;
        }

        public async Task<BasketPositionResponseDTO> AddToBasket(BasketPositionRequestDTO item)
        {
            var existingBasketItem = await _context.BasketPositions
                .FirstOrDefaultAsync(bp => bp.ProductID == item.ProductId && bp.UserID == item.UserId);

            if (existingBasketItem != null)
            {
                existingBasketItem.Amount += item.Amount;
            }
            else
            {
                var newBasketItem = new BasketPosition
                {
                    ProductID = item.ProductId,
                    UserID = item.UserId,
                    Amount = item.Amount
                };
                _context.BasketPositions.Add(newBasketItem);
            }

            await _context.SaveChangesAsync();

            return new BasketPositionResponseDTO
            {
                ProductId = item.ProductId,
                UserId = item.UserId,
                Amount = item.Amount
            };
        }

        public async Task<bool> UpdateBasketItemQuantity(int productId, int userId, int quantity)
        {
            var basketItem = await _context.BasketPositions
                .FirstOrDefaultAsync(bp => bp.ProductID == productId && bp.UserID == userId);

            if (basketItem == null) return false;

            basketItem.Amount = quantity;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveBasketItem(int productId, int userId)
        {
            var basketItem = await _context.BasketPositions
                .FirstOrDefaultAsync(bp => bp.ProductID == productId && bp.UserID == userId);

            if (basketItem == null) return false;

            _context.BasketPositions.Remove(basketItem);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
