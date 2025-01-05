using PawPal.ViewModel;

namespace PawPal.Views;

public partial class EditProfilePage : ContentPage
{
	public EditProfilePage(EditProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}