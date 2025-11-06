using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using LibraryManagement.Application.Interfaces;

//Setting Up the DI Container
var services = new ServiceCollection();

// Load configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

services.AddLogging(builder =>
    builder.AddSerilog(dispose: true));

// Register dependencies
services.AddSingleton<IBookRepository, InMemoryBookRepository>();
services.AddSingleton<IBookService, BookService>();
services.AddSingleton<LibraryManagementService>();

//Building the Service Provider and Resolving Dependencies
var provider = services.BuildServiceProvider();
var libraryService = provider.GetRequiredService<LibraryManagementService>();

bool exit = false;

while (!exit)
{
    Console.WriteLine("\n===== Library Management System =====");
    Console.WriteLine("1. Add a new book");
    Console.WriteLine("2. Update an existing book");
    Console.WriteLine("3. Delete a book");
    Console.WriteLine("4. List all books");
    Console.WriteLine("5. View details of a specific book");
    Console.WriteLine("6. Exit");
    Console.Write("Select an option: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await libraryService.AddBookAsync();
                break;
            case "2":
                await libraryService.UpdateBookAsync();
                break;
            case "3":
                await libraryService.DeleteBookAsync();
                break;
            case "4":
                await libraryService.ListBooksAsync();
                break;
            case "5":
                await libraryService.ViewBookAsync();
                break;
            case "6":
                exit = true;
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "An unhandled exception occurred.");
        Console.WriteLine($"Error: {ex.Message}");
    }
}
