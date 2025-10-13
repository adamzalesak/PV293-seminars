using Library.DataAccess.Entities;

namespace Library.DataAccess.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetActiveLoanByBookIdAsync(Guid bookId);
    Task<bool> HasActiveLoanAsync(Guid borrowerId, Guid bookId);
}