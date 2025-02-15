# 🐾 PawPal - Pet Care Organizer

PawPal is a .NET MAUI mobile application designed to help pet owners manage their pets' tasks, medical records, and vet contacts efficiently with SQLiteAsyncConnection for responsive database operations.

## Features

- Task Management: Create, edit, and manage pet-related tasks (e.g., feeding, grooming, vet appointments).
- Recurring Tasks: Set up recurring tasks with customizable intervals.
- Vet Contacts: Maintain a directory of veterinarians for quick access.
- Notifications: Get reminders for scheduled tasks.
- Offline Support: Uses SQLite for local data storage.

## Technologies Used

- .NET MAUI for cross-platform development
- MVVM Architecture for a clean separation of concerns
- SQLite (via Room-like Database Service) for local data persistence
- Dependency Injection (DI) for better maintainability
- Shell Navigation for seamless navigation

## Screenshots

Home Screen: Displays the list of pets and an overview of tasks.
![Home Screen](screenshot/home.png)

Task Screen: Allows users to view and manage tasks assigned to each pet.
![Task Screen](screenshot/tasks.png)

## Getting Started

To get started with PawPal, follow these instructions:

### Prerequisites

Ensure you have the following installed on your machine:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet)
- Visual Studio 2022 or later with **.NET MAUI** workload

### Installation

1. Clone this repository to your local machine:

   ```bash
   git clone https://github.com/iNoles/PawPal.git
   ```

2. Open the project in Visual Studio.

3. Restore the dependencies:

   ```bash
   dotnet restore
   ```

4. Build and run the application:

   ```bash
   dotnet build
   dotnet run
   ```

5. The app should launch on your device or emulator, allowing you to add pets, assign tasks, and track them.

## Contribution

If you'd like to contribute to PawPal, feel free to fork the repository, create a new branch, and submit a pull request with your changes.

### Steps for Contributing:
1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add your feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a pull request
