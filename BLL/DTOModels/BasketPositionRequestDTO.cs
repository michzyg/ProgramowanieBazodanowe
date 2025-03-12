using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class BasketPositionRequestDTO
    {
        public int ProductId { get; init; }
        public int UserId { get; init; }
        public int Amount { get; init; }
    }
}
