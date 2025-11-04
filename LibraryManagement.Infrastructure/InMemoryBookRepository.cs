using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using System.Collections.Concurrent;


namespace LibraryManagement.Infrastructure.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly ConcurrentDictionary<Guid, Book> _books = new();

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            return Task.FromResult(_books.Values.AsEnumerable());
        }

        public Task<Book?> GetByIdAsync(Guid id)
        {
            _books.TryGetValue(id, out var book);
            return Task.FromResult(book);
        }

        public Task AddAsync(Book book)
        {
            _books.TryAdd(book.Id, book);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Book book)
        {
            _books[book.Id] = book;
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var removed = _books.TryRemove(id, out _);
            return Task.FromResult(removed);
        }
    }
}
