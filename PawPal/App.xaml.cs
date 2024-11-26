namespace PawPal;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		// Request notification permissions when the app starts
		RequestNotificationPermissionAsync();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	private static async void RequestNotificationPermissionAsync()
	{
		var status = await Permissions.RequestAsync<Permissions.PostNotifications>();

		if (status == PermissionStatus.Granted)
		{
			Console.WriteLine("Notification permission granted.");
		}
		else
		{
			Console.WriteLine("Notification permission denied.");
		}
	}
}