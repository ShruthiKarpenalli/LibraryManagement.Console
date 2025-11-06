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

        [Theory]
        [InlineData("1234567890123", true)] // Valid ISBN
        [InlineData("123", false)]          // Invalid ISBN
        public async Task AddBookAsync_ShouldValidateISBN(string isbn, bool expectedResult)
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Test Book", Author = "Test Author", ISBN = isbn };
            if (expectedResult)
            {
                _mockRepo.Setup(r => r.AddAsync(book)).Returns(Task.CompletedTask);
            }

            // Act
            var result = await _service.AddBookAsync(book);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), expectedResult ? Times.Once() : Times.Never());
        }

        [Theory]
        [InlineData("1234567890123", true)] // Valid ISBN
        [InlineData("123", false)]          // Invalid ISBN
        public async Task UpdateBookAsync_ShouldValidateISBN(string isbn, bool expectedResult)
        {
            // Arrange
            var book = new Book { Id = Guid.NewGuid(), Title = "Test Book", Author = "Test Author", ISBN = isbn };
            if (expectedResult)
            {
                _mockRepo.Setup(r => r.UpdateAsync(book)).Returns(Task.CompletedTask);
            }

            // Act
            var result = await _service.UpdateBookAsync(book);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Book>()), expectedResult ? Times.Once() : Times.Never());
        }

        [Theory]
        [InlineData(true)]  // Book exists
        [InlineData(false)] // Book does not exist
        public async Task DeleteBookAsync_ShouldHandleExistence(bool bookExists)
        {
            // Arrange
            var bookId = Guid.NewGuid();
            _mockRepo.Setup(r => r.DeleteAsync(bookId)).ReturnsAsync(bookExists);

            // Act
            var result = await _service.DeleteBookAsync(bookId);

            // Assert
            Assert.Equal(bookExists, result);
            _mockRepo.Verify(r => r.DeleteAsync(bookId), Times.Once());
        }

        [Theory]
        [InlineData(true)]  // Book exists
        [InlineData(false)] // Book does not exist
        public async Task GetBookByIdAsync_ShouldHandleExistence(bool bookExists)
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = bookExists ? new Book { Id = bookId, Title = "Test Book", ISBN = "1234567890123" } : null;
            _mockRepo.Setup(r => r.GetByIdAsync(bookId)).ReturnsAsync(book);

            // Act
            var result = await _service.GetBookByIdAsync(bookId);

            // Assert
            if (bookExists)
            {
                Assert.NotNull(result);
                Assert.Equal(bookId, result!.Id);
            }
            else
            {
                Assert.Null(result);
            }
            _mockRepo.Verify(r => r.GetByIdAsync(bookId), Times.Once());
        }
    }
}
