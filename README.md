# LibraryManagement.Console

A simple console-based Library Management System implemented in C#. This application enables users to manage a collection of books and perform common library operations through a command-line interface.

## Features

- Add, update, or remove books from the library.
- Search for books by Id.
- View the complete list of available books.
- Simple, text-based menu for easy navigation.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or higher recommended)
- A C# compatible IDE or code editor (e.g., Visual Studio, Visual Studio Code)

### Building and Running

1. **Clone the repository:**

   ```bash
   git clone https://github.com/ShruthiKarpenalli/LibraryManagement.Console.git
   cd LibraryManagement.Console
   ```

2. **Build the project:**

   ```bash
   dotnet build
   ```

3. **Run the application:**

   ```bash
   dotnet run --project LibraryManagement.Console
   ```

## Usage

Upon running the application, you will be presented with a menu of options to manage books in the library. Follow the prompts to add, search, issue, or return books.

## Project Structure

```
LibraryManagement.Console/
├── Models/            # Contains class definitions for Book, User, etc.
├── Services/          # Contains business logic for library operations
├── Program.cs         # Entry point of the application
├── README.md          # Project documentation
```

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request for any bug fixes or enhancements.

## Author

[Shruthi Karpenalli](https://github.com/ShruthiKarpenalli)