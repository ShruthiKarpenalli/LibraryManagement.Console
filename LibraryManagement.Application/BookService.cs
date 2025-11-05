using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository repository, ILogger<BookService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            _logger.LogInformation("Fetching all books");
            return await _repository.GetAllAsync();
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching book by ID: {Id}", id);
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> AddBookAsync(Book book)
        {
            if (!IsValidIsbn(book.ISBN)) { 
                _logger.LogWarning("Invalid ISBN: {ISBN}", book.ISBN);
                return false;
            }

            await _repository.AddAsync(book);
            _logger.LogInformation("Added book: {Title}, ISBN: {ISBN}", book.Title, book.ISBN);
            return true;
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            if (!IsValidIsbn(book.ISBN)) {
                _logger.LogWarning("Invalid ISBN: {ISBN}", book.ISBN);
                return false;
            }

            await _repository.UpdateAsync(book);
            _logger.LogInformation("Updated book: {Title}", book.Title);
            return true;
        }

        public async Task<bool> DeleteBookAsync(Guid id)
        {
            var success = await _repository.DeleteAsync(id);
            if (!success)
            {
                _logger.LogWarning("Attempted to delete a book with ID {Id}, but it does not exist.", id);
            }
            _logger.LogInformation("Deleted book with ID: {Id}", id);
            return success;
        }

        private bool IsValidIsbn(string isbn)
        {
            return Regex.IsMatch(isbn, @"^\d{13}$");
        }
    }
}



