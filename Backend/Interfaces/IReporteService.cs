// En una carpeta "Services"
using Backend.Models;
using Backend.Interfaces;
using Backend.Repositories;

namespace Backend.Services
{
    public interface IReporteService
    {
        Task<Report> CrearReporteAsync(Report nuevoReporte);
        Task<Report?> ObtenerReportePorIdAsync(int id);
        Task<IEnumerable<Report>> ObtenerReportesDeUsuarioAsync(int usuarioId);
    }

    public class ReporteService : IReporteService
    {
        // Inyectamos el repositorio CONCRETO porque necesitamos un método específico de él.
        private readonly ReporteRepository _reporteRepository;

        // Constructor modificado que inyecta el repositorio concreto.
        public ReporteService(ReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
        }

        public async Task<Report> CrearReporteAsync(Report reporte)
        {
            // Lógica de negocio
            reporte.CreatedAt = DateTime.UtcNow;
            reporte.Estado = "Enviado";

            // Usamos el repositorio genérico a través del concreto
            await _reporteRepository.AgregarAsync(reporte);
            await _reporteRepository.GuardarCambiosAsync();
            return reporte;
        }

        public async Task<Report?> ObtenerReportePorIdAsync(int id)
        {
            // Usamos el método genérico del repositorio
            return await _reporteRepository.ObtenerPorIdAsync(id);
        }

        // Implementación del método que faltaba, usando el método específico del repositorio concreto.
        public async Task<IEnumerable<Report>> ObtenerReportesDeUsuarioAsync(int usuarioId)
        {
            return await _reporteRepository.ObtenerReportsPorUsuarioIdAsync(usuarioId);
        }
    }
   
}
