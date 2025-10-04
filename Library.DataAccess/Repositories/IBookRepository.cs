using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
    Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
    Task<Book?> GetBookByIsbnAsync(string isbn);
}