using ApiCC.Data; // Tu DbContext
using ApiCC.Models; // Tu modelo User

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}