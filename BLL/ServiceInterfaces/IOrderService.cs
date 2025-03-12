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
        IEnumerable<OrderResponseDTO> GetOrders(int? orderId, bool? isPaid, string sortBy, bool ascending);
        OrderResponseDTO GetOrderDetails(int orderId);
        OrderResponseDTO GenerateOrder(int userId);
        bool PayOrder(int orderId, decimal amount);
    }
}
