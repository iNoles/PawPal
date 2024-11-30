# PawPal

PawPal is a mobile application built using .NET MAUI designed to help pet owners manage their pets and their tasks efficiently. It allows users to add pets, assign tasks to pets, and keep track of pet-related activities with ease.

## Features

- **Add Pets**: Add new pets with their names, species, and date of birth.
- **Manage Pet Tasks**: Assign and track tasks for each pet.
- **Task Scheduling**: Set reminders for tasks related to pets.
- **Notification Support**: The app uses local notifications to remind users of upcoming pet tasks by their due date.

## Screenshots

![Home Screen](screenshots/maui-desktop.png)

## Technologies Used

-  **.NET MAUI**: A cross-platform mobile app framework for building iOS, Android, and Windows apps. MAUI enables a single codebase for all platforms.
-  **C#**: Primary programming language used for development, allowing for strong typing and modern object-oriented programming features.
-  **XAML**: A declarative language for building user interfaces, primarily used with .NET-based applications.
-  **SQLite**: A lightweight, serverless SQL database used for persisting pet and task data locally on the device.

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
