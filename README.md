# PawPal

PawPal is a mobile application built using .NET MAUI designed to help pet owners manage their pets and their tasks efficiently. It allows users to add pets, assign tasks to pets, and keep track of pet-related activities with ease.

## Features

- **Add Pets**: Add new pets with their names, species, and date of birth
- **Manage Pet Tasks**: Assign and track tasks for each pet
- **Task Scheduling**: Set reminders for tasks related to pets
- **Notification Support**: The app uses local notifications to remind users of upcoming pet tasks to schedule by due date

## Technologies Used

- **.NET MAUI**: Cross-platform mobile app framework for building iOS, Android, and Windows apps
- **Entity Framework Core**: ORM for database operations using SQLite
- **C#**: Primary programming language for development
- **XAML**: For building the UI

## Getting Started

To get started with PawPal, follow these instructions:

### Prerequisites

Ensure you have the following installed on your machine:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet)
- Visual Studio 2022 or later with **.NET MAUI** workload
- SQLite (used for local data storage)

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

5. The app should launch on your device/emulator, allowing you to add pets, assign tasks, and track them.

## Features

### 1. Add Pets

Users can add new pets by providing their name, species, and date of birth. Pets are stored in an SQLite database for persistent storage.

### 2. Assign Tasks to Pets

Each pet can have multiple tasks. Tasks can be added to a pet and tracked with a due date.

### 3. Task Notifications

The app uses local notifications to remind users of upcoming pet tasks.

## Contribution

If you'd like to contribute to PawPal, feel free to fork the repository, create a new branch, and submit a pull request with your changes.

### Steps for Contributing:
1. Fork the repository
2. Create a new branch (`git checkout -b feature/your-feature`)
3. Commit your changes (`git commit -am 'Add your feature'`)
4. Push to the branch (`git push origin feature/your-feature`)
5. Open a pull request
