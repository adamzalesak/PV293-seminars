using Library.Domain.Aggregates;

namespace Library.Application.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken);
}