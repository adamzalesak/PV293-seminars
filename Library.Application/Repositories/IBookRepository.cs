using Library.Domain.Aggregates;

namespace Library.Application.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetAllBooksWithAuthorsAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid authorId);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book?> GetBookByIsbnAsync(string isbn);
}