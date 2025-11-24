using Frontend.Services;
using Frontend.ViewModels;
using Frontend.Views;
using Microsoft.Extensions.Logging;

namespace Frontend.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		
		            // --- REGISTRO DE SERVICIOS (DEPENDENCY INJECTION) ---
            builder.Services.AddHttpClient<IReporteApiService, ReporteApiService>();
            builder.Services.AddTransient<NuevoReporteViewModel>();
            builder.Services.AddTransient<NuevoReportePage>();

            builder.Services.AddSingleton<AppShell>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
