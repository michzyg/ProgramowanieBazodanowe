using System.Data;
using Microsoft.Data.SqlClient;
using BLL.DTOModels.GroupDTOs;
using BLL.DTOModels.ProductDTOs;
using BLL.ServiceInterfaces;
using Dapper;

namespace BLL_DB.Services
{
    public class ProductServiceDb : IProductService
    {
        private readonly string _connectionString;

        public ProductServiceDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<ProductResponseDTO>> GetProducts(string? nameFilter, string? groupNameFilter, int? groupIdFilter, string? sortBy, bool sortOrder, bool includeInactive)
        {
            using var connection = CreateConnection();

            var query = @"
        SELECT * FROM View_ProductsWithFullGroupHierarchy
        WHERE (@IncludeInactive = 1 OR IsActive = 1)
          AND (@NameFilter IS NULL OR Name LIKE '%' + @NameFilter + '%')
          AND (@GroupNameFilter IS NULL OR GroupName LIKE '%' + @GroupNameFilter + '%')
          AND (@GroupIdFilter IS NULL OR GroupID = @GroupIdFilter)";

            query += sortBy switch
            {
                "name" => sortOrder ? " ORDER BY Name ASC" : " ORDER BY Name DESC",
                "price" => sortOrder ? " ORDER BY Price ASC" : " ORDER BY Price DESC",
                "group" => sortOrder ? " ORDER BY GroupName ASC" : " ORDER BY GroupName DESC",
                _ => ""
            };

            return await connection.QueryAsync<ProductResponseDTO>(query, new
            {
                NameFilter = nameFilter,
                GroupNameFilter = groupNameFilter,
                GroupIdFilter = groupIdFilter,
                IncludeInactive = includeInactive ? 1 : 0
            });
        }


        public async Task AddProduct(ProductRequestDTO productDto)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("AddNewProduct", new
            {
                productDto.Name,
                productDto.Price,
                productDto.GroupID
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task ChangeProductStatus(int productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("ChangeProductStatus", new
            {
                ProductID = productId
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteProduct(int productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("DeleteProduct", new
            {
                ProductID = productId
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<GroupResponseDTO>> GetGroups(int? parentId, string? sortBy, bool sortOrder)
        {
            using var connection = CreateConnection();

            var query = @"
                SELECT ID AS Id, Name,
                       (SELECT COUNT(*) FROM ProductGroups sg WHERE sg.ParentID = g.ID) AS HasChildren
                FROM ProductGroups g
                WHERE (@ParentID IS NULL AND g.ParentID IS NULL)
                   OR (g.ParentID = @ParentID)";

            query += sortBy switch
            {
                "name" => sortOrder ? " ORDER BY Name ASC" : " ORDER BY Name DESC",
                _ => ""
            };

            return await connection.QueryAsync<GroupResponseDTO>(query, new
            {
                ParentID = parentId
            });
        }

        public async Task AddGroup(GroupRequestDTO groupRequestDTO)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("AddGroup", new
            {
                groupRequestDTO.Name,
                ParentID = groupRequestDTO.ParentId
            }, commandType: CommandType.StoredProcedure);
        }
    }
}
