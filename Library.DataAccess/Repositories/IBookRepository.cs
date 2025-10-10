using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid authorId);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book?> GetBookByIsbnAsync(string isbn);
}