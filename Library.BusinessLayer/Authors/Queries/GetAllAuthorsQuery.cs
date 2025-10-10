using Library.BusinessLayer.CQRS;
using Library.BusinessLayer.Dtos;
using Library.DataAccess.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.BusinessLayer.Authors.Queries;

public class GetAllAuthorsQuery : IQuery<List<AuthorDto>>;

public class GetAllAuthorsQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
{
    public async Task<List<AuthorDto>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.Authors
            .Select(author => new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                BirthDate = author.BirthDate,
                Country = author.Country,
                TotalBooksPublished = author.TotalBooksPublished,
                LastPublishedDate = author.LastPublishedDate,
                MostPopularGenre = author.MostPopularGenre,
            })
            .ToListAsync(cancellationToken);
    }
}