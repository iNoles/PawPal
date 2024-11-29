using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace PawPal;

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

		try
		{
			builder.UseLocalNotification();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"LocalNotification initialization failed: {ex.Message}");
		}

		// Register ViewModel
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<AddTaskPageViewModel>();

		builder.Services.AddTransient<AddTaskPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
