using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using Serilog;


public class LibraryManagementService
{
    private readonly IBookService _bookService;

    public LibraryManagementService(IBookService bookService)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }

    public async Task AddBookAsync()
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
        var success = await _bookService.AddBookAsync(book);
        Log.Information("Book added: {Title}, ISBN: {ISBN}", title, isbn);

        Console.WriteLine(success ? "Book added successfully!" : "Invalid ISBN. Must be 13 digits.");
    }

    public async Task UpdateBookAsync()
    {
        Console.Write("Enter Book ID to update: ");
        if (!Guid.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var book = await _bookService.GetBookByIdAsync(id);
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

        var success = await _bookService.UpdateBookAsync(book);

        Console.WriteLine(success ? "Book updated successfully!" : "Invalid ISBN. Must be 13 digits.");
    }

    public async Task DeleteBookAsync()
    {
        Console.Write("Enter Book ID to delete: ");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            var success = await _bookService.DeleteBookAsync(id);
            Console.WriteLine(success ? "Book deleted successfully." : "Book not found. Deletion failed.");
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
    }

    public async Task ListBooksAsync()
    {
        var books = await _bookService.GetAllBooksAsync();
        foreach (var b in books)
        {
            Console.WriteLine($"{b.Id} | {b.Title} by {b.Author} ({b.PublishedYear}) - ISBN: {b.ISBN}");
        }
    }

    public async Task ViewBookAsync()
    {
        Console.Write("Enter Book ID: ");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            var book = await _bookService.GetBookByIdAsync(id);
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
}

