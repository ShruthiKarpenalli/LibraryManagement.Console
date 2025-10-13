using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.DTOs
{
    /// <summary>
    /// Data Transfer Object for book operations
    /// Used to transfer data between layers without exposing domain entities
    /// </summary>
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }

        /// <summary>
        /// Converts DTO to domain entity
        /// </summary>
        public Book ToEntity()
        {
            return new Book
            {
                Id = Id,
                Title = Title,
                Author = Author,
                ISBN = ISBN,
                PublishedYear = PublishedYear
            };
        }

        /// <summary>
        /// Creates DTO from domain entity
        /// </summary>
        public static BookDto FromEntity(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                PublishedYear = book.PublishedYear
            };
        }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Author: {Author}, ISBN: {ISBN}, Year: {PublishedYear}";
        }
    }
}