using PawPal.ViewModel;

namespace PawPal;

public partial class CalendarPage : ContentPage
{
	public CalendarPage(CalendarViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}