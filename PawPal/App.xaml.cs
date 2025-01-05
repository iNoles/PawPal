using PawPal.Services;

namespace PawPal;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        _ = NotificationService.InitializeNotificationsAsync();
	}

	protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
