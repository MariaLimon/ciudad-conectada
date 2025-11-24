using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.ViewModels
{
    public class NuevoReporteViewModel : INotifyPropertyChanged
    {
        private readonly IReporteApiService _reporteApiService;
        private string _title;
        private string _description;
        private string _location;

        public NuevoReporteViewModel(IReporteApiService reporteApiService)
        {
            _reporteApiService = reporteApiService;
            CrearReporteCommand = new Command(async () => await OnCrearReporte());
        }

        public string Title { get => _title; set => SetProperty(ref _title, value); }
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        public string Location { get => _location; set => SetProperty(ref _location, value); }
        public ICommand CrearReporteCommand { get; }

        private async Task OnCrearReporte()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Description))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El título y la descripción son obligatorios.", "OK");
                return;
            }

            var nuevoReporte = new Report
            {
                UserId = 1, // Valor de ejemplo
                ServiceId = 1, // Valor de ejemplo
                Title = this.Title,
                Description = this.Description,
                Location = this.Location,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _reporteApiService.CrearReporteAsync(nuevoReporte);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Reporte creado correctamente.", "OK");
                Title = string.Empty; Description = string.Empty; Location = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo crear el reporte: {ex.Message}", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return false;
            backingStore = value; OnPropertyChanged(propertyName); return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}