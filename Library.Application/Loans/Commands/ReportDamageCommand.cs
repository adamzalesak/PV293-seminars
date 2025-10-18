using Library.Application.Repositories;
using Library.Domain.Common.CQRS;
using Library.Domain.ValueObjects;
using MediatR;

namespace Library.Application.Loans.Commands;

public class ReportDamageCommand : ICommand<Guid>
{
    public Guid LoanId { get; set; }
    public string DamageDescription { get; set; } = string.Empty;
    public Money DamageCost { get; set; } = null!;
}

public class ReportDamageCommandHandler(
    ILoanRepository loanRepository
    ) : IRequestHandler<ReportDamageCommand, Guid>
{
    public async Task<Guid> Handle(ReportDamageCommand request, CancellationToken cancellationToken)
    {
        var loan = await loanRepository.GetByIdAsync(request.LoanId);
        if (loan == null)
        {
            throw new InvalidOperationException($"Loan with ID {request.LoanId} not found");
        }

        loan.ReportDamage(request.DamageDescription, request.DamageCost);

        return loan.Id;
    }
}