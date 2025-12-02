using Backend.Models;

namespace Backend.Interfaces
{
    public interface IServiceService // Nombra la interfaz de forma única
    {
        Task<IEnumerable<Service>> ObtenerTodosLosServiciosAsync();
        Task<Service?> ObtenerServicioPorIdAsync(int id);
        Task<Service> CrearServicioAsync(Service nuevoServicio);
    }
}