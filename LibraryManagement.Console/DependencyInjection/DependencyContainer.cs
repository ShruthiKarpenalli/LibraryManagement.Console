using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Repositories;

namespace LibraryManagement.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Dependency injection container for managing dependencies
    /// Part of the Infrastructure layer - handles cross-cutting concerns
    /// </summary>
    public static class DependencyContainer
    {
        /// <summary>
        /// Configures and returns the service dependencies
        /// </summary>
        public static (IBookService BookService) ConfigureServices()
        {
            // Infrastructure layer
            IBookRepository repository = new InMemoryBookRepository();
            
            // Application layer
            IBookService bookService = new BookService(repository);
                                  
            return (bookService);
        }
    }
}