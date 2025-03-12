using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_EF.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly WebstoreContext _context;

        public OrderService(WebstoreContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDTO> GenerateOrder(int userId)
        {
            var basketItems = await _context.BasketPositions
                .Where(bp => bp.UserID == userId)
                .ToListAsync();

            if (!basketItems.Any()) throw new InvalidOperationException("Basket is empty.");

            var newOrder = new Order
            {
                UserID = userId,
                Date = DateTime.UtcNow,
                IsPaid = false
            };

            _context.Orders.Add(newOrder);

            var orderPositions = basketItems.Select(bp => new OrderPosition
            {
                Order = newOrder,
                ProductId = bp.ProductID,
                Amount = bp.Amount,
                Price = _context.Products.FirstOrDefault(p => p.ID == bp.ProductID)?.Price ?? 0
            }).ToList();

            _context.OrderPositions.AddRange(orderPositions);

            _context.BasketPositions.RemoveRange(basketItems);
            await _context.SaveChangesAsync();

            return new OrderResponseDTO
            {
                Id = newOrder.ID,
                UserId = userId,
                Date = newOrder.Date,
                IsPaid = false
            };
        }

        public async Task<bool> PayOrder(int orderId, decimal amount)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;


            decimal totalOrderValue = await _context.OrderPositions
                .Where(op => op.OrderID == orderId)
                .SumAsync(op => (decimal)op.Price * op.Amount);

            if (amount < totalOrderValue)
                throw new InvalidOperationException("Insufficient amount.");

            order.IsPaid = true;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<OrderResponseDTO>> GetOrders(int? orderId, bool? isPaid, string sortBy, bool ascending)
        {
            var query = _context.Orders.AsQueryable();

            if (orderId.HasValue)
                query = query.Where(o => o.ID == orderId.Value);

            if (isPaid.HasValue)
                query = query.Where(o => o.IsPaid == isPaid.Value);

            query = sortBy switch
            {
                "value" => ascending ? query.OrderBy(o => o.ID) : query.OrderByDescending(o => o.ID),
                "date" => ascending ? query.OrderBy(o => o.Date) : query.OrderByDescending(o => o.Date),
                _ => query
            };

            return await query.Select(o => new OrderResponseDTO
            {
                Id = o.ID,
                UserId = o.UserID,
                Date = o.Date,
                IsPaid = o.IsPaid
            }).ToListAsync();
        }

        public async Task<IEnumerable<OrderPositionResponseDTO>> GetOrderPositions(int orderId)
        {
            return await _context.OrderPositions
                .Where(op => op.OrderID == orderId)
                .Select(op => new OrderPositionResponseDTO
                {
                    OrderId = op.OrderID,
                    ProductId = op.ProductId,
                    Amount = op.Amount,
                    Price = op.Price
                })
                .ToListAsync();
        }

        public Task<OrderResponseDTO> GetOrderDetails(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
