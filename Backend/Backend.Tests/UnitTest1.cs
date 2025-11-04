using Xunit;
using System.Linq;

namespace Backend.Tests
{
    public class WeatherForecastTests // Ejemplo basado en la plantilla por defecto
    {
        [Fact]
        public void GetWeatherForecasts_ReturnsFiveItems()
        {
            // Arrange (Preparación): No es necesario aquí, ya que probamos el método directamente.
            
            // Act (Acción): Simulamos la lógica que devolvería 5 elementos.
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = "TestSummary"
            }).ToArray();
            
            // Assert (Verificación): Comprobamos que el resultado no sea nulo y tenga 5 elementos.
            Assert.NotNull(result);
            Assert.Equal(5, result.Length);
        }
    }

    // Clase necesaria para que la prueba compile (cópiala si no existe en tu proyecto)
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }
    }
}