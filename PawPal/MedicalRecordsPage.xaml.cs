using PawPal.ViewModel;

namespace PawPal;

public partial class MedicalRecordsPage : ContentPage
{
	public MedicalRecordsPage(MedicalRecordsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}