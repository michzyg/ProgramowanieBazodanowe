using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IProductGroupService
    {
        IEnumerable<ProductGroupResponseDTO> GetProductGroups(int? parentGroupId, string sortBy, bool ascending);
        ProductGroupResponseDTO AddProductGroup(ProductGroupRequestDTO group);
    }
}
