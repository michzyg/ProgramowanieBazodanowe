using System.Data;
using Microsoft.Data.SqlClient;
using BLL.DTOModels.OrderDTOs;
using BLL.ServiceInterfaces;
using Dapper;

namespace BLL_DB.Services
{
    public class OrderServiceDb : IOrderService
    {
        private readonly string _connectionString;

        public OrderServiceDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<OrderResponseDTO>> GetOrder(int? idFilter, bool? paidFilter, string? sortBy, bool sortOrder)
        {
            using var connection = CreateConnection();

            string baseQuery = @"
                SELECT o.ID AS OrderID, o.Date AS OrderDate, o.IsPaid,
                       SUM(op.Price * op.Amount) AS TotalPrice
                FROM Orders o
                JOIN OrderPositions op ON o.ID = op.OrderID
                WHERE (o.ID = @IdFilter OR @IdFilter IS NULL)
                  AND (o.IsPaid = @PaidFilter OR @PaidFilter IS NULL)
                GROUP BY o.ID, o.Date, o.IsPaid";

            string orderBy = sortBy switch
            {
                "value" => $" ORDER BY TotalPrice {(sortOrder ? "ASC" : "DESC")}",
                "date" => $" ORDER BY OrderDate {(sortOrder ? "ASC" : "DESC")}",
                "id" => $" ORDER BY OrderID {(sortOrder ? "ASC" : "DESC")}",
                _ => ""
            };

            var query = baseQuery + orderBy;

            return await connection.QueryAsync<OrderResponseDTO>(query, new
            {
                IdFilter = idFilter,
                PaidFilter = paidFilter
            });
        }

        public async Task<OrderDetailsResponseDTO> GetOrderDetails(int orderID)
        {
            using var connection = CreateConnection();

            var items = await connection.QueryAsync<OrderItemDTO>(@"
                SELECT p.Name AS ProductName, op.Price, op.Amount,
                       (op.Price * op.Amount) AS TotalValue
                FROM OrderPositions op
                JOIN Products p ON op.ProductId = p.ID
                WHERE op.OrderID = @OrderID", new { OrderID = orderID });

            return new OrderDetailsResponseDTO
            {
                OrderPositions = items.ToList()
            };
        }

        public async Task PayOrder(int orderId, double amount)
        {
            using var connection = CreateConnection();

            var result = await connection.ExecuteScalarAsync<int>(
                "EXEC PayOrder @OrderID, @Amount",
                new { OrderID = orderId, Amount = amount });

            if (result == -1)
                throw new InvalidOperationException("Order not found.");
            if (result == -2)
                throw new InvalidOperationException("Order already paid.");
            if (result == -3)
                throw new InvalidOperationException("Provided amount doesn't match order total.");
        }
    }
}
