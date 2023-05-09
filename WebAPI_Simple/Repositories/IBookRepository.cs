using WebAPI_Simple.Models.Domain;
using WebAPI_Simple.Models.DTO;

namespace WebAPI_Simple.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);
        BookWithAuthorAndPublisherDTO GetBookById(int id);
        AddBookRequestDTO AddBook(AddBookRequestDTO addBookRequestDTO);
        AddBookRequestDTO? UpdateBookById (int id, AddBookRequestDTO BookDTO);
        Books? DeleteBookById (int id);
    }
}
