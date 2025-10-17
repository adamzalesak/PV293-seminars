using Library.Application.CQRS;
using Library.Application.Repositories;
using MediatR;

namespace Library.Application.Loans.Commands;

public class ExtendLoanDueDateCommand : ICommand<Guid>
{
    public Guid LoanId { get; set; }
    public int AdditionalDays { get; set; }
}

public class ExtendLoanDueDateCommandHandler(
    ILoanRepository loanRepository
    ) : IRequestHandler<ExtendLoanDueDateCommand, Guid>
{
    public async Task<Guid> Handle(ExtendLoanDueDateCommand request, CancellationToken cancellationToken)
    {
        var loan = await loanRepository.GetByIdAsync(request.LoanId);
        if (loan == null)
        {
            throw new InvalidOperationException($"Loan with ID {request.LoanId} not found");
        }

        loan.ExtendDueDate(request.AdditionalDays);

        return loan.Id;
    }
}