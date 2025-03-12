using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOModels
{
    public class ProductGroupResponseDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int? ParentId { get; init; }
    }
}
