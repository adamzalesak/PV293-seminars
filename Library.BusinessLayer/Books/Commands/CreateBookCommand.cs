using Library.BusinessLayer.Books.Events;
using Library.BusinessLayer.CQRS;
using Library.DataAccess.Entities;
using Library.DataAccess.Repositories;
using MediatR;

namespace Library.BusinessLayer.Books.Commands;

public class CreateBookCommand : ICommand<Guid>
{
    public string Title { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string Genre { get; set; } = string.Empty;
}

public class CreateBookCommandHandler(
    IBookRepository bookRepository,
    IMediator mediator)
    : IRequestHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            AuthorId = command.AuthorId,
            ISBN = command.ISBN,
            Year = command.Year,
            Pages = command.Pages,
            Genre = command.Genre,
        };

        bookRepository.Add(book);

        await mediator.Publish(new BookCreatedEvent
        {
            BookId = book.Id,
            AuthorId = book.AuthorId,
            Genre = book.Genre
        }, cancellationToken);

        return book.Id;
    }
}