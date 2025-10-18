using Library.Application.Dtos;
using Library.Application.Repositories;
using Library.Domain.Common.CQRS;
using MediatR;

namespace Library.Application.Authors.Queries;

public class GetAllAuthorsQuery : IQuery<List<AuthorDto>>;

public class GetAllAuthorsQueryHandler(
    IAuthorRepository authorRepository
    ) : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
{
    public async Task<List<AuthorDto>> Handle(GetAllAuthorsQuery query, CancellationToken cancellationToken)
    {
        var authors = await authorRepository.GetAllAsync(cancellationToken);

        return authors
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
            .ToList();
    }
}