using Library.BusinessLayer.CQRS;
using Library.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.BusinessLayer.Books.Events;

public class BookCreatedEvent : IDomainEvent
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }
    public string Genre { get; set; } = string.Empty;
}

public class BookCreatedEventHandler(ApplicationDbContext dbContext) : IDomainEventHandler<BookCreatedEvent>
{
    public async Task Handle(BookCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Update author statistics
        var author = await dbContext.Authors
            .FirstOrDefaultAsync(a => a.Id == notification.AuthorId, cancellationToken);

        if (author == null)
            return;

        // Increment total books published
        author.TotalBooksPublished++;
        author.LastPublishedDate = DateTime.UtcNow;

        // Update the author's most popular genre based on all their books
        var mostPopularGenre = await dbContext.Books
            .Where(b => b.AuthorId == author.Id)
            .GroupBy(b => b.Genre)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrEmpty(mostPopularGenre))
        {
            author.MostPopularGenre = mostPopularGenre;
        }

        dbContext.Authors.Update(author);
    }
}