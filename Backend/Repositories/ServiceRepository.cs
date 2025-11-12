// Repositories/ServiceRepository.cs
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Interfaces;

public class ServiceRepository : IRepository<Service>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Service> _dbSet;

    // Inyectamos el DbContext para poder acceder a la base de datos
    public ServiceRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Service>();
    }

    public async Task AgregarAsync(Service entidad)
    {
        await _dbSet.AddAsync(entidad);
    }

    public void Actualizar(Service entidad)
    {
        _dbSet.Attach(entidad);
        _context.Entry(entidad).State = EntityState.Modified;
    }

    public void Eliminar(Service entidad)
    {
        if (_context.Entry(entidad).State == EntityState.Detached)
        {
            _dbSet.Attach(entidad);
        }
        _dbSet.Remove(entidad);
    }

    public async Task<Service?> ObtenerPorIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<Service>> ObtenerTodosAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<int> GuardarCambiosAsync()
    {
        return await _context.SaveChangesAsync();
    }
}