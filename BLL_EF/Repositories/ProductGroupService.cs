using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_EF.Repositories
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly WebstoreContext _context;

        public ProductGroupService(WebstoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductGroupResponseDTO>> GetProductGroups(int? parentGroupId, string sortBy, bool ascending)
        {
            var query = _context.ProductGroups.AsQueryable();

            if (parentGroupId.HasValue)
                query = query.Where(g => g.ParentID == parentGroupId.Value);
            else
                query = query.Where(g => g.ParentID == null);

            query = sortBy switch
            {
                "name" => ascending ? query.OrderBy(g => g.Name) : query.OrderByDescending(g => g.Name),
                _ => query
            };

            return await query.Select(g => new ProductGroupResponseDTO
            {
                Id = g.ID,
                Name = g.Name,
                ParentId = g.ParentID
            }).ToListAsync();
        }

        public async Task<ProductGroupResponseDTO> AddProductGroup(ProductGroupRequestDTO group)
        {
            var newGroup = new ProductGroup
            {
                Name = group.Name,
                ParentID = group.ParentId
            };

            _context.ProductGroups.Add(newGroup);
            await _context.SaveChangesAsync();

            return new ProductGroupResponseDTO
            {
                Id = newGroup.ID,
                Name = newGroup.Name,
                ParentId = newGroup.ParentID
            };
        }
    }
}
