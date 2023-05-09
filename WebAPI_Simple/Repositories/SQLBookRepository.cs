using Microsoft.EntityFrameworkCore;
using WebAPI_Simple.Data;
using WebAPI_Simple.Models.Domain;
using WebAPI_Simple.Models.DTO;

namespace WebAPI_Simple.Repositories
{
    public class SQLBookRepository : IBookRepository
    {
        private readonly AppDBContext _dbContext;
        public SQLBookRepository (AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<BookWithAuthorAndPublisherDTO> GetAllBooks(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            var allBooks = _dbContext.Books.Select(Books => new BookWithAuthorAndPublisherDTO()
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.IsRead ? Books.DateRead.Date : null,
                Rate = Books.IsRead ? Books.Rate.CompareTo(Books.Rate) : null,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publishers.Name,
                AuthorNames = Books.Books_Authors.Select(n => n.Authors.Fullname).ToList()

            }).AsQueryable();
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = allBooks.Where(x => x.Title.Contains(filterQuery));
                }
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
                {
                    allBooks = isAscending ? allBooks.OrderBy(x => x.Title) : allBooks.OrderByDescending(x => x.Title);
                }
            }    

            //Paging
            var skipResults = (pageNumber  - 1) * pageSize;
            return allBooks.Skip(skipResults).Take(pageSize).ToList();

        }
        public BookWithAuthorAndPublisherDTO GetBookById(int id) 
        {
            var bookwithmain = _dbContext.Books.Where(n => n.Id == id);
            if (bookwithmain == null)
            {
                return null;
            }
            //map domain model to DTOs
            var bookwithIdDTO = bookwithmain.Select(Books => new BookWithAuthorAndPublisherDTO()
            {
                Id = Books.Id,
                Title = Books.Title,
                Description = Books.Description,
                IsRead = Books.IsRead,
                DateRead = Books.DateRead,
                Rate = Books.Rate,
                Genre = Books.Genre,
                CoverUrl = Books.CoverUrl,
                PublisherName = Books.Publishers.Name,
                AuthorNames = Books.Books_Authors.Select(n => n.Authors.Fullname).ToList()
            }).FirstOrDefault();
            return bookwithIdDTO;
        }

        public AddBookRequestDTO AddBook (AddBookRequestDTO addBookRequestDTO)
        {
            var BookDomainModel = new Books
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = (DateTime)addBookRequestDTO.DateRead,
                Rate = (int)addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = addBookRequestDTO.PublisherID,
            };

            //Use domain model to add book
            _dbContext.Books.Add(BookDomainModel);
            _dbContext.SaveChanges();
            foreach (var id in addBookRequestDTO.AuthorIds)
            {
                var _book_author = new Books_Authors()
                {
                    BookId = BookDomainModel.Id,
                    AuthorId = id,
                };
                _dbContext.Books_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return addBookRequestDTO;
        }

        public AddBookRequestDTO? UpdateBookById(int id, AddBookRequestDTO BookDTO)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(n => n.Id == id);
            if (bookDomain != null)
            {
                bookDomain.Title = BookDTO.Title;
                bookDomain.Description = BookDTO.Description;
                bookDomain.IsRead = BookDTO.IsRead;
                bookDomain.DateRead = (DateTime)BookDTO.DateRead;
                bookDomain.Rate = (int)BookDTO.Rate;
                bookDomain.Genre = BookDTO.Genre;
                bookDomain.CoverUrl = BookDTO.CoverUrl;
                bookDomain.DateAdded = (DateTime)BookDTO.DateAdded;
                bookDomain.PublisherID = (int)BookDTO.PublisherID;
                _dbContext.SaveChanges();
            }
            var AuthorDomain = _dbContext.Books_Authors.Where(a => a.BookId == id).ToList();
            if (AuthorDomain != null)
            {
                _dbContext.Books_Authors.RemoveRange(AuthorDomain);
                _dbContext.SaveChanges();
            }
            foreach (var authorid in BookDTO.AuthorIds)
            {
                var _book_author = new Books_Authors
                {
                    BookId = id,
                    AuthorId = authorid
                };
                _dbContext.Books_Authors.Add(_book_author);
                _dbContext.SaveChanges();
            }
            return BookDTO;
        }

        public Books? DeleteBookById(int id)
        {
            var bookDomain = _dbContext.Books.FirstOrDefault(b => b.Id == id);
            if (bookDomain != null)
            {
                _dbContext.Books.Remove(bookDomain);
                _dbContext.SaveChanges();
            }
            return bookDomain;
        }
    }
}
