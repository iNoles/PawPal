using PawPal.ViewModel;

namespace PawPal;

public partial class AddTaskPage : ContentPage
{
    public AddTaskPage(AddTaskPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}