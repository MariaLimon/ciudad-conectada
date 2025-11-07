using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApiCC.Data;

namespace Backend.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            // 1. Obtenemos el HostBuilder que la factory crea por defecto
            var builder = base.CreateHostBuilder();
            
            // 2. Usamos ese HostBuilder para establecer el entorno de "Testing"
            builder.UseEnvironment("Testing");
            
            // 3. Devolvemos el HostBuilder modificado
            return builder;
        }

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            // Como el entorno está configurado en CreateHostBuilder, aquí solo necesitamos
            // configurar los servicios para usar la BD en memoria.
            builder.ConfigureServices(services =>
            {
                // Eliminamos el DbContext existente (el de PostgreSQL)
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Añadimos el DbContext con la BD en memoria
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Opcional: Crear la base de datos
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}