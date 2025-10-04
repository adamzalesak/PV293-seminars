using Library.BusinessLayer.Dtos;

namespace Library.BusinessLayer.Services;

public interface IBookService
{
    Task<List<BookDto>> GetAllBooksAsync();
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<BookDto> CreateBookAsync(BookDto bookDto);
    Task<BookDto?> UpdateBookAsync(int id, BookDto bookDto);
    Task<bool> DeleteBookAsync(int id);
}