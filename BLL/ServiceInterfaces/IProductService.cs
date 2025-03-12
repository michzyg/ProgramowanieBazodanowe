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
        Task<IEnumerable<ProductResponseDTO>> GetProducts(string? name, int? groupId, bool includeInactive, string sortBy, bool ascending);
        Task<ProductResponseDTO> AddProduct(ProductRequestDTO product);
        Task<bool> UpdateProduct(int productId, ProductRequestDTO updatedProduct);
        Task<bool> DeactivateProduct(int productId);
        Task<bool> ActivateProduct(int productId);
        Task<bool> DeleteProduct(int productId);

    }
}
