using Library.Domain.Entities;

namespace Library.Application.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
