namespace LibraryManagement.Domain.Entities
{
    /// <summary>
    /// Represents a book in the library system
    /// </summary>
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Title: {Title}, Author: {Author}, ISBN: {ISBN}, Year: {PublishedYear}";
        }
    }
}