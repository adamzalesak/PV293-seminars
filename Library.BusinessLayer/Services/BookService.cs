using Library.BusinessLayer.Dtos;
using Library.DataAccess.Entities;
using Library.DataAccess.Repositories;
using Library.DataAccess.UnitOfWork;

namespace Library.BusinessLayer.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookService(IBookRepository bookRepository, IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<BookDto>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToDto).ToList();
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return book != null ? MapToDto(book) : null;
    }

    public async Task<BookDto> CreateBookAsync(BookDto bookDto)
    {
        if (string.IsNullOrWhiteSpace(bookDto.Title))
            throw new ArgumentException("Book title is required", nameof(bookDto.Title));

        if (string.IsNullOrWhiteSpace(bookDto.Author))
            throw new ArgumentException("Book author is required", nameof(bookDto.Author));

        var book = MapToEntity(bookDto);
        _bookRepository.Add(book);
        await _unitOfWork.CompleteAsync();

        return MapToDto(book);
    }

    public async Task<BookDto?> UpdateBookAsync(int id, BookDto bookDto)
    {
        if (string.IsNullOrWhiteSpace(bookDto.Title))
            throw new ArgumentException("Book title is required", nameof(bookDto.Title));

        if (string.IsNullOrWhiteSpace(bookDto.Author))
            throw new ArgumentException("Book author is required", nameof(bookDto.Author));

        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null)
            return null;

        existingBook.Title = bookDto.Title;
        existingBook.Author = bookDto.Author;
        existingBook.ISBN = bookDto.ISBN;
        existingBook.Year = bookDto.Year;
        existingBook.Pages = bookDto.Pages;
        existingBook.Genre = bookDto.Genre;

        _bookRepository.Update(existingBook);
        await _unitOfWork.CompleteAsync();

        return MapToDto(existingBook);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null)
            return false;

        _bookRepository.Remove(book);
        await _unitOfWork.CompleteAsync();
        return true;
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