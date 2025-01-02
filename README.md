# PawPal

PawPal is a mobile application built using .NET MAUI designed to help pet owners manage their pets and their tasks efficiently. It allows users to add pets, assign tasks to pets, and keep track of pet-related activities with ease.

## Features

- **Add and Manage Pets**: Add pets with details such as name, species, and date of birth. Edit or remove pet information as needed.
- **Multi-Pet Support**: Seamlessly manage multiple pets within the app, with separate profiles and tasks for each pet.
- **Task Management**: Assign and organize tasks for each pet, with options to set due dates and priorities.
- **Task Reminders**: Schedule reminders for pet-related tasks, ensuring you never miss an important task or event.
- **Notification Support**: The app uses local notifications to remind users of upcoming pet tasks based on their due dates.
- **Medical Records Management**: Record and manage pet medical history, including vaccination records, prescriptions, and notes from veterinarians.
- **Customizable Medical Records**: Easily add, edit, or delete medical records for pets, with fields for record type, date, notes, and more.
- **Dark Mode**: Built-in support for Dark Mode, ensuring a visually comfortable experience in low-light conditions.

## Screenshots

Home Screen: Displays the list of pets and an overview of tasks.
![Home Screen](screenshot/home.png)

Task Screen: Allows users to view and manage tasks assigned to each pet.
![Task Screen](screenshot/tasks.png)

## Technologies Used

-  **.NET MAUI**: A cross-platform mobile app framework for building iOS, Android, and Windows apps. MAUI enables a single codebase for all platforms.
-  **C#**: Primary programming language used for development, allowing for strong typing and modern object-oriented programming features.
-  **XAML**: A declarative language for building user interfaces, primarily used with .NET-based applications.
-  **SQLite**: A lightweight, serverless SQL database used for persisting pet and task data locally on the device.
-  **LINQ**: Used for querying and manipulating collections of pet and task data, enabling clean and efficient operations such as sorting, filtering, and aggregating data.
- **Visual Studio**: Recommended IDE for streamlined development and debugging with .NET MAUI.

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
