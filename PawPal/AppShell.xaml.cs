﻿namespace PawPal;

using PawPal.Views;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for pages not listed in the ShellContent
		Routing.RegisterRoute(nameof(PetDetailsPage), typeof(PetDetailsPage));
		Routing.RegisterRoute(nameof(EditProfilePage), typeof(EditProfilePage));
		Routing.RegisterRoute(nameof(TaskPage), typeof(TaskPage));
		Routing.RegisterRoute(nameof(MedicalRecordsPage), typeof(MedicalRecordsPage));
		Routing.RegisterRoute(nameof(AddEditVetContactPage), typeof(AddEditVetContactPage));
	}
}
