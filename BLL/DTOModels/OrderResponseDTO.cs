using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class OrderResponseDTO
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public DateTime Date { get; init; }
        public bool IsPaid { get; init; }
    }

}
