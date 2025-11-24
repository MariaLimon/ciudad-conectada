using System.Text.Json;
using Frontend.Models;

namespace Frontend.Services
{
    public class ReporteApiService : IReporteApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "http://localhost:5156/api"; // Cambia si es necesario

        public ReporteApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(ApiBaseUrl);
        }

        public async Task<Report> CrearReporteAsync(Report nuevoReporte)
        {
            var reporteJson = JsonSerializer.Serialize(nuevoReporte);
            var content = new StringContent(reporteJson, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Reports", content);

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Report>(responseStream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new Report();
        }
    }
}