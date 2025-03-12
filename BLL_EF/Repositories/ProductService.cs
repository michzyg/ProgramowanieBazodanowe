using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_EF.Repositories
{
    public class ProductService : IProductService
    {
        private readonly WebstoreContext _context;

        public ProductService(WebstoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProducts(string? name, string? groupName, int? groupId, bool includeInactive, string sortBy, bool ascending)
        {
            var query = _context.Products
                .Include(p => p.Group)
                .ThenInclude(pg => pg.ParentGroup)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(groupName))
                query = query.Where(p => p.Group != null && p.Group.Name.Contains(groupName));

            if (groupId.HasValue)
                query = query.Where(p => p.GroupID == groupId.Value);

            if (!includeInactive)
                query = query.Where(p => p.IsActive);

            query = sortBy switch
            {
                "name" => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                _ => query
            };

            var result = await query.Select(p => new ProductResponseDTO
            {
                Id = p.ID,
                Name = p.Name,
                Price = p.Price,
                GroupName = GetFullGroupNameAsync(p.ID).Result
            }).ToListAsync();

            return result;
        }

        public async Task<ProductResponseDTO> AddProduct(ProductRequestDTO product)
        {
            var newProduct = new Product
            {
                Name = product.Name,
                Price = product.Price,
                GroupID = product.GroupId,
                IsActive = true
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            return new ProductResponseDTO
            {
                Id = newProduct.ID,
                Name = newProduct.Name,
                Price = newProduct.Price,
                GroupId = newProduct.GroupID,
                IsActive = newProduct.IsActive,
                GroupName = await GetFullGroupNameAsync(newProduct.ID)
            };
        }

        private async Task<string> GetFullGroupNameAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Group)
                .ThenInclude(pg => pg.ParentGroup)
                .FirstOrDefaultAsync(p => p.ID == productId);

            if (product == null || product.Group == null)
            {
                throw new ArgumentException("Product not found or doesn't have a group.");
            }

            var groupNames = new List<string>();
            var currentGroup = product.Group;

            while (currentGroup != null)
            {
                groupNames.Add(currentGroup.Name);

                currentGroup = await _context.ProductGroups
                    .FirstOrDefaultAsync(pg => pg.ID == currentGroup.ParentID);
            }

            return string.Join(" / ", groupNames);
        }

        public async Task<bool> DeactivateProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            product.IsActive = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<IEnumerable<ProductResponseDTO>> GetProducts(string? name, int? groupId, bool includeInactive, string sortBy, bool ascending)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProduct(int productId, ProductRequestDTO updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
