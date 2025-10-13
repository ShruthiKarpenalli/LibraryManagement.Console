using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;

var services = new ServiceCollection();

// 1. Resolve project root (LibraryManagement.Console folder)
var projectRoot = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

// 2. Create Logs folder in project root
var logFolder = Path.Combine(projectRoot, "Logs");
Directory.CreateDirectory(logFolder); // ensures folder exists

// Load configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File(Path.Combine(logFolder, "log-.txt"), rollingInterval: RollingInterval.Day)
    .CreateLogger();

services.AddLogging(builder =>
    builder.AddSerilog(dispose: true));

// Register dependencies
services.AddSingleton<IBookRepository, InMemoryBookRepository>();
services.AddSingleton<BookService>();

var provider = services.BuildServiceProvider();
var bookService = provider.GetRequiredService<BookService>();

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
                await AddBookAsync(bookService);
                break;
            case "2":
                await UpdateBookAsync(bookService);
                break;
            case "3":
                await DeleteBookAsync(bookService);
                break;
            case "4":
                await ListBooksAsync(bookService);
                break;
            case "5":
                await ViewBookAsync(bookService);
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

static async Task AddBookAsync(BookService service)
{
    Console.Write("Title: ");
    var title = Console.ReadLine();
    Console.Write("Author: ");
    var author = Console.ReadLine();
    Console.Write("ISBN (13 digits): ");
    var isbn = Console.ReadLine();
    Console.Write("Year: ");
    int.TryParse(Console.ReadLine(), out int year);

    var book = new Book { Id = Guid.NewGuid(), Title = title!, Author = author!, ISBN = isbn!, PublishedYear = year };
    var success = await service.AddBookAsync(book);
    Log.Information("Book added: {Title}, ISBN: {ISBN}", title, isbn);

    Console.WriteLine(success ? "Book added successfully!" : "Invalid ISBN. Must be 13 digits.");
}

static async Task UpdateBookAsync(BookService service)
{
    Console.Write("Enter Book ID to update: ");
    if (!Guid.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var book = await service.GetBookByIdAsync(id);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.Write("New Title: ");
    book.Title = Console.ReadLine()!;
    Console.Write("New Author: ");
    book.Author = Console.ReadLine()!;
    Console.Write("New ISBN (13 digits): ");
    book.ISBN = Console.ReadLine()!;
    Console.Write("New Year: ");
    int.TryParse(Console.ReadLine(), out var year);
    book.PublishedYear = year;

    await service.UpdateBookAsync(book);
    Console.WriteLine("Book updated successfully.");
}

static async Task DeleteBookAsync(BookService service)
{
    Console.Write("Enter Book ID to delete: ");
    if (Guid.TryParse(Console.ReadLine(), out var id))
    {
        await service.DeleteBookAsync(id);
        Console.WriteLine("Book deleted successfully.");
    }
    else
    {
        Console.WriteLine("Invalid ID.");
    }
}

static async Task ListBooksAsync(BookService service)
{
    var books = await service.GetAllBooksAsync();
    foreach (var b in books)
    {
        Console.WriteLine($"{b.Id} | {b.Title} by {b.Author} ({b.PublishedYear}) - ISBN: {b.ISBN}");
    }
}

static async Task ViewBookAsync(BookService service)
{
    Console.Write("Enter Book ID: ");
    if (Guid.TryParse(Console.ReadLine(), out var id))
    {
        var book = await service.GetBookByIdAsync(id);
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        Console.WriteLine($"\nTitle: {book.Title}\nAuthor: {book.Author}\nISBN: {book.ISBN}\nYear: {book.PublishedYear}");
    }
    else
    {
        Console.WriteLine("Invalid ID.");
    }
}
