using Library.BusinessLayer.Dtos;
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
        var createdBook = await _bookRepository.AddAsync(book);
        return MapToDto(createdBook);
    }

    public async Task<BookDto?> UpdateBookAsync(int id, BookDto bookDto)
    {
        if (string.IsNullOrWhiteSpace(bookDto.Title))
            throw new ArgumentException("Book title is required", nameof(bookDto.Title));

        if (string.IsNullOrWhiteSpace(bookDto.Author))
            throw new ArgumentException("Book author is required", nameof(bookDto.Author));

        var book = MapToEntity(bookDto);
        var updatedBook = await _bookRepository.UpdateAsync(id, book);
        return updatedBook != null ? MapToDto(updatedBook) : null;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteAsync(id);
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