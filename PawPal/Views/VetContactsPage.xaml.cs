using PawPal.ViewModel;

namespace PawPal.Views;

public partial class VetContactsPage : ContentPage
{
	public VetContactsPage(VetContactsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}