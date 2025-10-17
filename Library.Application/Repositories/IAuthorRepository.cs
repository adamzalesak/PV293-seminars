using Library.Domain.Entities;

namespace Library.Application.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    Task<IEnumerable<Author>> GetAllAsync(CancellationToken cancellationToken);
}