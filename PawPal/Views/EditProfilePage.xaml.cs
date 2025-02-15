using PawPal.ViewModel;

namespace PawPal.Views;

public partial class EditProfilePage : ContentPage
{
    private readonly EditProfileViewModel _viewModel;
    private readonly int _petId;

    public EditProfilePage(EditProfileViewModel viewModel, int petId)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _petId = petId;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync(_petId);
    }
}
