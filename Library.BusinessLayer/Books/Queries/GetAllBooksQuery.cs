using Library.BusinessLayer.CQRS;
using Library.BusinessLayer.Dtos;
using Library.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.BusinessLayer.Books.Queries;

public class GetAllBooksQuery : IQuery<List<BookDto>>;

public class GetAllBooksQueryHandler(ApplicationDbContext dbContext) : IQueryHandler<GetAllBooksQuery, List<BookDto>>
{
    public async Task<List<BookDto>> Handle(GetAllBooksQuery query, CancellationToken cancellationToken)
    {
        return await dbContext.Books
            .Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.Name,
                ISBN = book.ISBN,
                Year = book.Year,
                Pages = book.Pages,
                Genre = book.Genre,
            })
            .ToListAsync(cancellationToken);
    }
}