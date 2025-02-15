using PawPal.ViewModel;

namespace PawPal.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;
    
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
