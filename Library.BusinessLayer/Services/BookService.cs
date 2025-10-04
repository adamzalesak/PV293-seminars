using Library.DataAccess.Entities;
using Library.DataAccess.Repositories;

namespace Library.BusinessLayer.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title is required", nameof(book.Title));

        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Book author is required", nameof(book.Author));

        return await _bookRepository.AddAsync(book);
    }

    public async Task<Book?> UpdateBookAsync(int id, Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title is required", nameof(book.Title));

        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Book author is required", nameof(book.Author));

        return await _bookRepository.UpdateAsync(id, book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteAsync(id);
    }
}