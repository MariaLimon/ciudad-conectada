using Backend.Models;
using Backend.Interfaces;
using BCrypt.Net; 

namespace Backend.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var userFromDb = await _repository.GetByConditionAsync(u => u.Email == email);
            if (userFromDb == null) return null;

            return BCrypt.Net.BCrypt.Verify(password, userFromDb.Password) ? userFromDb : null;
        }
    }
}