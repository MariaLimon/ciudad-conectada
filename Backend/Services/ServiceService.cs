using Backend.Models;
using Backend.Interfaces;

namespace Backend.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepository;

        // Inyectamos el repositorio genérico
        public ServiceService(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> CrearServicioAsync(Service servicio)
        {
            // Aquí podrías agregar lógica de negocio, por ejemplo:
            // - Verificar que no exista un servicio con el mismo tipo y compañía.
            // - Formatear el nombre de la compañía a mayúsculas.

            await _serviceRepository.AgregarAsync(servicio);
            await _serviceRepository.GuardarCambiosAsync();
            return servicio;
        }

        public async Task<Service?> ObtenerServicioPorIdAsync(int id)
        {
            return await _serviceRepository.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Service>> ObtenerTodosLosServiciosAsync()
        {
            return await _serviceRepository.ObtenerTodosAsync();
        }
    }
}