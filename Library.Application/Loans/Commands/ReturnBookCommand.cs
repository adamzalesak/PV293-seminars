using Library.Application.Repositories;
using Library.Domain.Common.CQRS;
using MediatR;

namespace Library.Application.Loans.Commands;

public record ReturnBookCommand : ICommand
{
    public Guid LoanId { get; init; }
}

public class ReturnBookCommandHandler(
    ILoanRepository loanRepository
) : IRequestHandler<ReturnBookCommand>
{
    public async Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var loan = await loanRepository.GetByIdAsync(request.LoanId) 
          ?? throw new InvalidOperationException($"Loan with ID {request.LoanId} not found");

        loan.Return();
    }
}
