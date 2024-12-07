using PawPal.ViewModel;

namespace PawPal;

public partial class PetDetailsPage : ContentPage
{
	public PetDetailsPage(PetDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}