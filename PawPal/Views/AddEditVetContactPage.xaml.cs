using PawPal.ViewModel;

namespace PawPal.Views;

public partial class AddEditVetContactPage : ContentPage
{
	public AddEditVetContactPage(AddVetContactViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}