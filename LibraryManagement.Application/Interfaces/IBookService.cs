using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Interfaces
{
    /// <summary>
    /// Application service interface for book operations
    /// Defines the use cases for the application
    /// </summary>
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<bool> AddBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<bool> DeleteBookAsync(Guid id);
    }
}