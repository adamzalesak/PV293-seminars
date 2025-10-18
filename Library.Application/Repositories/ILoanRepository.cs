using Library.Domain.Aggregates.Loan;

namespace Library.Application.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
    Task<bool> HasActiveLoanAsync(Guid borrowerId, Guid bookId);
}