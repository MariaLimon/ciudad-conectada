namespace Backend.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<T>> ObtenerTodosAsync();
        Task AgregarAsync(T entidad);
        void Actualizar(T entidad); 
        void Eliminar(T entidad);
        Task<int> GuardarCambiosAsync();
    }
}

