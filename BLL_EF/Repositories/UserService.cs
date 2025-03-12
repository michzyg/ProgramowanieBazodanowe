using BLL.DTOModels;
using BLL.ServiceInterfaces;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLL_EF.Repositories
{
    public class UserService : IUserService
    {
        private readonly WebstoreContext _context;

        private static UserResponseDTO? _loggedInUser;

        public UserService(WebstoreContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDTO?> Login(UserRequestDTO loginRequest)
        {
            if (_loggedInUser != null)
                return _loggedInUser;

            var foundUser = await _context.Users
                .Where(u => u.Login == loginRequest.Login && u.Password == loginRequest.Password && u.IsActive)
                .FirstOrDefaultAsync();

            if (foundUser == null)
                throw new UnauthorizedAccessException("Invalid credentials.");


            _loggedInUser = new UserResponseDTO
            {
                Id = foundUser.ID,
                Login = foundUser.Login,
                Type = foundUser.Type.ToString(),
                IsActive = foundUser.IsActive,
                GroupId = foundUser.GroupID ?? 0
            };

            return _loggedInUser;
        }

        public Task<bool> Logout(int userId)
        {
            if (_loggedInUser == null || _loggedInUser.Id != userId)
                return Task.FromResult(false);

            _loggedInUser = null;
            return Task.FromResult(true);
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
