using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDTO>> GetOrders(int? orderId, bool? isPaid, string sortBy, bool ascending);
        Task<OrderResponseDTO> GetOrderDetails(int orderId);
        Task<OrderResponseDTO> GenerateOrder(int userId);
        Task<bool> PayOrder(int orderId, decimal amount);
    }
}
