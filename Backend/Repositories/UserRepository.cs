// En Backend/Repositories/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Interfaces;
using System.Linq.Expressions;

namespace Backend.Repositories
{
    public class UserRepository : IRepository<User>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<User> _dbSet;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<User>();
    }

    public async Task AddAsync(User entity) => await _dbSet.AddAsync(entity);

    public async Task<IEnumerable<User>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<User?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<User?> GetByConditionAsync(Expression<Func<User, bool>> predicate)
        => await _dbSet.FirstOrDefaultAsync(predicate);

    public void Update(User entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(User entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}

}