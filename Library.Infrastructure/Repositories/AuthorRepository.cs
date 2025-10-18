using Library.Application.Repositories;
using Library.Domain.Aggregates;
using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Repositories;

public class AuthorRepository(ApplicationDbContext context) : Repository<Author>(context), IAuthorRepository
{
    private ApplicationDbContext ApplicationDbContext => (ApplicationDbContext)Context;
}