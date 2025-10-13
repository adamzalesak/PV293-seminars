using System.Security.Claims;
using Library.BusinessLayer.Loans.Commands;
using Library.DataAccess.Constants;
using MediatR;

namespace Library.API.Endpoints;

public static class LoansEndpoints
{
    public static void MapLoansEndpoints(this WebApplication app)
    {
        var loans = app.MapGroup("/api/loans")
            .WithTags("Loans")
            .WithOpenApi();

        loans.MapPost("/", CheckOutBook)
            .WithName("CheckOutBook")
            .Accepts<CheckOutBookRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization(policy => policy
                .RequireRole(UserRoles.Member, UserRoles.Librarian, UserRoles.Admin));
    }

    private static async Task<IResult> CheckOutBook(
        CheckOutBookRequest request,
        IMediator mediator,
        ClaimsPrincipal user)
    {
        var command = new CheckOutBookCommand
        {
            BookId = request.BookId,
            BorrowerId = request.BorrowerId,
            LoanDurationDays = request.LoanDurationDays ?? 14,
            User = user
        };

        var loanId = await mediator.Send(command);
        return Results.Created($"/api/loans/{loanId}", new { id = loanId });
    }
}

public record CheckOutBookRequest
{
    public Guid BookId { get; init; }
    public Guid? BorrowerId { get; init; }
    public int? LoanDurationDays { get; init; }
}