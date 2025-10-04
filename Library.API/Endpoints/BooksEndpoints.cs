using Library.API.Dtos;
using Library.BusinessLayer.Services;
using Library.DataAccess.Entities;

namespace Library.API.Endpoints;

public static class BooksEndpoints
{
    public static void MapBooksEndpoints(this WebApplication app)
    {
        app.MapGet("/api/books", GetBooks)
            .WithName("GetBooks")
            .WithOpenApi()
            .Produces<List<BookDto>>(StatusCodes.Status200OK);

        app.MapPost("/api/books", CreateBook)
            .WithName("CreateBook")
            .WithOpenApi()
            .Accepts<BookDto>("application/json")
            .Produces<BookDto>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> GetBooks(IBookService bookService)
    {
        var books = await bookService.GetAllBooksAsync();
        var bookDtos = books.Select(MapToDto).ToList();
        return Results.Ok(bookDtos);
    }

    private static async Task<IResult> CreateBook(BookDto bookDto, IBookService bookService)
    {
        try
        {
            var book = MapToEntity(bookDto);
            var createdBook = await bookService.CreateBookAsync(book);
            var createdBookDto = MapToDto(createdBook);
            return Results.Created($"/api/books/{createdBookDto.Id}", createdBookDto);
        }
        catch (ArgumentException ex)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                { ex.ParamName ?? "Error", new[] { ex.Message } }
            });
        }
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            ISBN = book.ISBN,
            Year = book.Year,
            Pages = book.Pages,
            Genre = book.Genre
        };
    }

    private static Book MapToEntity(BookDto bookDto)
    {
        return new Book
        {
            Id = bookDto.Id,
            Title = bookDto.Title,
            Author = bookDto.Author,
            ISBN = bookDto.ISBN,
            Year = bookDto.Year,
            Pages = bookDto.Pages,
            Genre = bookDto.Genre
        };
    }
}