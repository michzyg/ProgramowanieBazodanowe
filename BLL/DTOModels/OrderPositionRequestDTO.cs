using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class OrderPositionRequestDTO
    {
        public int OrderId { get; init; }
        public int ProductId { get; init; }
        public int Amount { get; init; }
        public double Price { get; init; }
    }
}
