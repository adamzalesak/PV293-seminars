using FluentValidation;
using Library.BusinessLayer.Books.Commands;
using Library.BusinessLayer.Books.Queries;
using Library.BusinessLayer.Dtos;
using MediatR;

namespace Library.API.Endpoints;

public static class BooksEndpoints
{
    public static void MapBooksEndpoints(this WebApplication app)
    {
        var books = app.MapGroup("/api/books")
            .WithTags("Books")
            .WithOpenApi();

        books.MapGet("", GetBooks)
            .WithName("GetBooks")
            .Produces<List<BookDto>>();

        books.MapPost("", CreateBook)
            .WithName("CreateBook")
            .Accepts<CreateBookCommand>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }

    private static async Task<IResult> GetBooks(IMediator mediator)
    {
        var query = new GetAllBooksQuery();
        var books = await mediator.Send(query);
        return Results.Ok(books);
    }

    private static async Task<IResult> CreateBook(CreateBookCommand command, IMediator mediator)
    {
        try
        {
            var id = await mediator.Send(command);
            return Results.Created($"/api/books/{id}", new { id });
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return Results.ValidationProblem(errors);
        }
    }
}