using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Repositories;
using Xunit;

namespace LibraryManagement.Tests.Infrastructure
{
    public class InMemoryBookRepositoryTests
    {
        private readonly IBookRepository _repository;

        public InMemoryBookRepositoryTests()
        {
            _repository = new InMemoryBookRepository();
        }

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book",
                Author = "Author 1",
                ISBN = "1234567890123"
            };

            await _repository.AddAsync(book);

            var allBooks = await _repository.GetAllAsync();
            Assert.Contains(allBooks, b => b.Id == book.Id && b.Title == "Test Book");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBook()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Test Book 2",
                Author = "Author 2",
                ISBN = "9876543210987"
            };

            await _repository.AddAsync(book);

            var result = await _repository.GetByIdAsync(book.Id);
            Assert.NotNull(result);
            Assert.Equal(book.Title, result!.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingBook()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                Author = "Author",
                ISBN = "1234567890123"
            };

            await _repository.AddAsync(book);

            book.Title = "Updated Title";
            await _repository.UpdateAsync(book);

            var updatedBook = await _repository.GetByIdAsync(book.Id);
            Assert.Equal("Updated Title", updatedBook!.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book To Delete",
                Author = "Author",
                ISBN = "1234567890123"
            };

            await _repository.AddAsync(book);
            await _repository.DeleteAsync(book.Id);

            var result = await _repository.GetByIdAsync(book.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            var book1 = new Book { Id = Guid.NewGuid(), Title = "Book1", ISBN = "1234567890123" };
            var book2 = new Book { Id = Guid.NewGuid(), Title = "Book2", ISBN = "9876543210987" };

            await _repository.AddAsync(book1);
            await _repository.AddAsync(book2);

            var allBooks = await _repository.GetAllAsync();
            Assert.Equal(2, allBooks.Count());
        }
    }
}
