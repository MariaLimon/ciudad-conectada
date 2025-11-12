using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using Backend.Interfaces;

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

        public async Task AgregarAsync(Report entidad)
        {
            await _dbSet.AddAsync(entidad);
        }

        public void Actualizar(Report entidad)
        {
            _dbSet.Attach(entidad);
            _context.Entry(entidad).State = EntityState.Modified;
        }

        public void Eliminar(Report entidad)
        {
            if (_context.Entry(entidad).State == EntityState.Detached)
            {
                _dbSet.Attach(entidad);
            }
            _dbSet.Remove(entidad);
        }

        public async Task<Report?> ObtenerPorIdAsync(int id)
        {
            return await _dbSet
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Report>> ObtenerTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Report>> ObtenerReportsPorUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet.Where(r => r.UserId == usuarioId).ToListAsync();
        }

        public async Task<int> GuardarCambiosAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
