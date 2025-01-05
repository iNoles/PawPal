using PawPal.ViewModel;

namespace PawPal.Views;

public partial class PetDetailsPage : ContentPage
{
	public PetDetailsPage(PetDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}