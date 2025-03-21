using System.Data;
using Microsoft.Data.SqlClient;
using BLL.DTOModels.BasketDTOs;
using BLL.DTOModels.OrderDTOs;
using BLL.ServiceInterfaces;
using Dapper;
using Model;

namespace BLL_DB.Services
{
    public class BasketServiceDb : IBasketService
    {
        private readonly string _connectionString;

        public BasketServiceDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task AddProductToBasket(BasketRequestDTO basketDto)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("AddProductToBasket", new
            {
                basketDto.UserID,
                basketDto.ProductID,
                basketDto.Amount
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task RemoveFromBasket(int userId, int productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("RemoveProductFromBasket", new
            {
                UserID = userId,
                ProductID = productId
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateBasketItem(int userId, int productId, int amount)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("UpdateBasketItem", new
            {
                UserID = userId,
                ProductID = productId,
                Amount = amount
            }, commandType: CommandType.StoredProcedure);
        }

        public async Task<OrderResponseDTO> CreateOrder(int userId)
        {
            using var connection = CreateConnection();

            var order = await connection.QueryFirstOrDefaultAsync<OrderResponseDTO>(
                "CreateOrder", new { UserID = userId }, commandType: CommandType.StoredProcedure);

            if (order == null)
                throw new InvalidOperationException("Order creation failed.");

            return order;
        }
    }
}
