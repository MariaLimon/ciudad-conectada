using Frontend.Models;

namespace Frontend.Services
{
    public interface IReporteApiService
    {
        Task<Report> CrearReporteAsync(Report nuevoReporte);
    }
}