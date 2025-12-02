using Backend.Models;
using Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Backend.Data;
namespace Backend.Repositories
{
    public class ServiceRepository : IRepository<Service>
    {
        private readonly ApplicationDbContext _context; // O tu nombre de contexto

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }

        public async Task<Service?> GetByConditionAsync(Expression<Func<Service, bool>> expression)
        {
            return await _context.Services.FirstOrDefaultAsync(expression);
        }


        public async Task AddAsync(Service entity)
        {
            await _context.Services.AddAsync(entity);
            await SaveChangesAsync();
        }

        public void Update(Service entity)
        {
            _context.Services.Update(entity);
            _context.SaveChanges(); // o await SaveChangesAsync si lo haces async
        }

        public void Delete(Service entity)
        {
            _context.Services.Remove(entity);
            _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
