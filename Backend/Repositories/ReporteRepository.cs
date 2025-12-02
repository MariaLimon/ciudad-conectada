// En Backend/Repositories/ReporteRepository.cs
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Interfaces;
using System.Linq.Expressions;

namespace Backend.Repositories
{
    public class ReporteRepository : IRepository<Report>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Report> _dbSet;

    public ReporteRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Report>();
    }

    public async Task AddAsync(Report entity) => await _dbSet.AddAsync(entity);

    public async Task<IEnumerable<Report>> GetAllAsync() 
        => await _dbSet.Include(r => r.User)
        .Include(r => r.Service1) 
        .ToListAsync();

    public async Task<Report?> GetByIdAsync(int id) 
        => await _dbSet.Include(r => r.User)
        .Include(r => r.Service1) 
        .FirstOrDefaultAsync(r => r.Id == id);

    public async Task<Report?> GetByConditionAsync(Expression<Func<Report, bool>> predicate)
        => await _dbSet.Include(r => r.User).FirstOrDefaultAsync(predicate);

    public void Update(Report entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(Report entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            _dbSet.Attach(entity);

        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<Report>> GetReportsAsync()
{
    return await _dbSet
        .Include(r => r.User)
        .Include(r => r.Service1)
        .ToListAsync();
}

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    // MÉTODOS PERSONALIZADOS (Opcionales)
    public async Task<IEnumerable<Report>> ObtenerReportsPorUsuarioIdAsync(int usuarioId)
        => await _dbSet.Where(r => r.UserId == usuarioId)
        .Include(r => r.User)
        .Include(r => r.Service1)
        .ToListAsync();
}

}