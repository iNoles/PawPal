using PawPal.ViewModel;

namespace PawPal.Views;

public partial class MedicalRecordsPage : ContentPage
{
	public MedicalRecordsPage(MedicalRecordsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}