using BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceInterfaces
{
    public interface IProductService
    {
        IEnumerable<ProductResponseDTO> GetProducts(string? name, int? groupId, bool includeInactive, string sortBy, bool ascending);
        ProductResponseDTO AddProduct(ProductRequestDTO product);
        bool UpdateProduct(int productId, ProductRequestDTO updatedProduct);
        bool DeactivateProduct(int productId);
        bool ActivateProduct(int productId);
        bool DeleteProduct(int productId);

    }
}
