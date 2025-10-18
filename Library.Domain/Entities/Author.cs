using Library.Domain.Common;

namespace Library.Domain.Entities;

public class Author : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;

    // Navigation property
    public List<Book> Books { get; set; } = new();

    // Statistics that need updating when books are added/removed
    public int TotalBooksPublished { get; set; }
    public DateTime? LastPublishedDate { get; set; }
    public string MostPopularGenre { get; set; } = string.Empty;

    // Constructor for EF Core
    public Author() : base() { }

    // Constructor with ID for seeding
    public Author(Guid id) : base(id) { }
}