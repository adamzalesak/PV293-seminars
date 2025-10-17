using Library.Application.Repositories;
using Library.Domain.Constants;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<string>> GetUserRolesByUserIdAsync(Guid userId)
    {
        return await ApplicationDbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(ApplicationDbContext.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name!)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await ApplicationDbContext.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(ApplicationDbContext.Roles,
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name!)
            .ToListAsync(cancellationToken);
    }

    private ApplicationDbContext ApplicationDbContext => (ApplicationDbContext)Context;
}
