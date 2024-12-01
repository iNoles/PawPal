
using PawPal.ViewModel;

namespace PawPal;

public partial class PetProfilePage : ContentPage, IQueryAttributable
{
	private readonly PetProfileViewModel _viewModel;

	public PetProfilePage()
	{
		InitializeComponent();
		_viewModel = new PetProfileViewModel();
		BindingContext = _viewModel;
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        // Parse query parameters and populate the SelectedPet
		var id = query["id"];
		var name = query["name"];
		var species = query["species"];
		var breed = query["breed"];
		var dob = DateTime.Parse((string)query["birthDate"]);
		var medical = query["medical"];
		_viewModel.SelectedPet = new Pet
		{
			Id = (int)id,
			Name = (string)name,
            Species = (string)species,
            Breed = (string)breed,
            DateOfBirth = dob,
            MedicalRecords = (string)medical
		};

    }
}