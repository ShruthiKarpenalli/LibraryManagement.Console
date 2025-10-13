using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryManagement.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _mockRepo;
        private readonly Mock<ILogger<BookService>> _mockLogger;
        private readonly BookService _service;

        public BookServiceTests()
        {
            _mockRepo = new Mock<IBookRepository>();
            _mockLogger = new Mock<ILogger<BookService>>();
            _service = new BookService(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task AddBookAsync_ShouldReturnTrue_WhenValidISBN()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Clean Code", Author = "Robert Martin", ISBN = "1234567890123" };
            _mockRepo.Setup(r => r.AddAsync(book)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddBookAsync(book);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task AddBookAsync_ShouldReturnFalse_WhenInvalidISBN()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Bad Code", Author = "Unknown", ISBN = "123" };

            // Act
            var result = await _service.AddBookAsync(book);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldReturnTrue_WhenValidISBN()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Old Title", Author = "X", ISBN = "1234567890123" };
            _mockRepo.Setup(r => r.UpdateAsync(book)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateBookAsync(book);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldReturnFalse_WhenInvalidISBN()
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Invalid ISBN", ISBN = "123" };

            // Act
            var result = await _service.UpdateBookAsync(book);

            // Assert
            Assert.False(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task DeleteBookAsync_ShouldCallRepository()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteBookAsync(id);

            // Assert
            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetBookByIdAsync_ShouldReturnBook_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedBook = new Book { Id = id, Title = "Book Title", ISBN = "1234567890123" };
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expectedBook);

            // Act
            var result = await _service.GetBookByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Book Title", result!.Title);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnAllBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new() { Id = Guid.NewGuid(), Title = "Book1", ISBN = "1234567890123" },
                new() { Id = Guid.NewGuid(), Title = "Book2", ISBN = "9876543210987" }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _service.GetAllBooksAsync();

            // Assert
            Assert.Equal(2, ((List<Book>)result).Count);
        }
    }
}
