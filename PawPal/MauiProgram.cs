using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "PawPal.db");
		builder.Services.AddDbContext<PetCareDbContext>(options => options.UseSqlite($"Data Source={dbPath}"));

		// Register Repository
        builder.Services.AddScoped<PetRepository>();

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
