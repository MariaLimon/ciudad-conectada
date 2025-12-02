using Backend.Models;
using Backend.Repositories;

namespace Backend.Services
{
    public interface IReporteService
    {
        Task<Report> CrearReporteAsync(Report nuevoReporte);
        Task<Report?> ObtenerReportePorIdAsync(int id);
        Task<IEnumerable<Report>> ObtenerReportesDeUsuarioAsync(int usuarioId);
        Task<IEnumerable<Report>> GetReportsAsync();
        Task<Report?> ActualizarEstadoReporteAsync(int id, string nuevoEstado);
    }

    public class ReporteService : IReporteService
    {
        private readonly ReporteRepository _reporteRepository; // SE MANTIENE porque necesitas métodos extra

        public ReporteService(ReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
        }

        public async Task<Report> CrearReporteAsync(Report reporte)
        {
            reporte.CreatedAt = DateTime.UtcNow;
            reporte.Estado = "Enviado";

            await _reporteRepository.AddAsync(reporte); // 🔥 corregido
            await _reporteRepository.SaveChangesAsync(); // 🔥 corregido
            return reporte;
        }

        public async Task<Report?> ObtenerReportePorIdAsync(int id)
        {
            return await _reporteRepository.GetByIdAsync(id); // 🔥 corregido
        }

        public async Task<IEnumerable<Report>> ObtenerReportesDeUsuarioAsync(int usuarioId)
        {
            return await _reporteRepository.ObtenerReportsPorUsuarioIdAsync(usuarioId); // método extra OK
        }

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            return await _reporteRepository.GetReportsAsync(); // extra OK
        }

        public async Task<Report?> ActualizarEstadoReporteAsync(int id, string nuevoEstado)
        {
            var reporte = await _reporteRepository.GetByIdAsync(id); // 🔥 corregido
            if (reporte == null) return null;

            reporte.Estado = nuevoEstado;
            await _reporteRepository.SaveChangesAsync(); // 🔥 corregido

            return reporte;
        }
    }
}
