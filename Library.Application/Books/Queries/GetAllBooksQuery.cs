using Library.Application.CQRS;
using Library.Application.Dtos;
using Library.Application.Repositories;

namespace Library.Application.Books.Queries;

public class GetAllBooksQuery : IQuery<List<BookDto>>;

public class GetAllBooksQueryHandler(
    IBookRepository bookRepository
    ) : IQueryHandler<GetAllBooksQuery, List<BookDto>>
{
    public async Task<List<BookDto>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);

        return books.Select(b => new BookDto
        {
            Id = b.Id,
            Title = b.Title,
            ISBN = b.ISBN,
            Year = b.Year,
            Pages = b.Pages,
            Genre = b.Genre,
            AuthorId = b.AuthorId,
            AuthorName = b.Author.Name
        }).ToList();
    }
}