using Backend.Models;
using Backend.Interfaces;

namespace Backend.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IRepository<Service> _serviceRepository;

        public ServiceService(IRepository<Service> serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<Service> CrearServicioAsync(Service servicio)
        {
            await _serviceRepository.AddAsync(servicio);  // 🔥 corregido
            await _serviceRepository.SaveChangesAsync();  // 🔥 corregido
            return servicio;
        }

        public async Task<Service?> ObtenerServicioPorIdAsync(int id)
        {
            return await _serviceRepository.GetByIdAsync(id); // 🔥 corregido
        }

        public async Task<IEnumerable<Service>> ObtenerTodosLosServiciosAsync()
        {
            return await _serviceRepository.GetAllAsync(); // 🔥 corregido
        }
    }
}
