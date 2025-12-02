using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserService
    {
        Task<User?> ValidateUserAsync(string email, string password);
        Task<User?> GetUserByIdAsync(int id);
        
    }
}
