using Library.BusinessLayer.CQRS;
using Library.DataAccess.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.BusinessLayer.Auth.Queries;

public class GetAllUsersQuery : IQuery<List<UserDto>>
{
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime MembershipDate { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class GetAllUsersQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await dbContext.Users
            .Select(user => new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MembershipDate = user.MembershipDate,
                Roles = dbContext.UserRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Join(dbContext.Roles,
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r.Name!)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return users;
    }
}