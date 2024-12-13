using Microsoft.Extensions.Logging;
using PawPal.ViewModel;
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
		builder.Services.AddTransient<CalendarViewModel>();
		builder.Services.AddTransient<PetDetailsViewModel>();
		builder.Services.AddTransient<EditProfileViewModel>();
		builder.Services.AddTransient<TaskPageViewModel>();
		builder.Services.AddTransient<MedicalRecordsViewModel>();

#if DEBUG
        builder.Logging.AddDebug(); // Adds debug logging in debug builds
#else
		builder.Logging.AddConsole(); // Optionally add console logging for other environments
#endif

		return builder.Build();
	}
}
