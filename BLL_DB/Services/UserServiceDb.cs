using System.Data;
using Microsoft.Data.SqlClient;
using BLL.DTOModels.UserDTOs;
using BLL.ServiceInterfaces;
using Dapper;

namespace BLL_DB.Services
{
    public class UserServiceDb : IUserService
    {
        private readonly string _connectionString;
        private static string? _userSession = null;

        public UserServiceDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<bool> Login(UserLoginRequestDTO userLoginRequestDTO)
        {
            using var connection = CreateConnection();

            var result = await connection.ExecuteScalarAsync<int>(
                "UserLogin",
                new
                {
                    userLoginRequestDTO.Login,
                    userLoginRequestDTO.Password
                },
                commandType: CommandType.StoredProcedure
            );

            if (result == 1)
            {
                _userSession = userLoginRequestDTO.Login;
                return true;
            }

            return false;
        }

        public Task Logout()
        {
            _userSession = null;
            return Task.CompletedTask;
        }
    }
}
