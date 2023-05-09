using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI_Simple.Data;
using WebAPI_Simple.Models.DTO;
using WebAPI_Simple.Models.Domain;
using System.Reflection.Metadata.Ecma335;
using WebAPI_Simple.Repositories;
using WebAPI_Simple.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI_Simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly AppDBContext? _dbContext;
        private readonly IBookRepository _bookRepository;
        public BookController(AppDBContext? dbContext, IBookRepository bookRepository)
        {
            this._dbContext = dbContext;
            this._bookRepository = bookRepository;
        }

        /* // Get http://localhost:port/api/get-all-books
         [HttpGet("Get-all-books")]
         public IActionResult GetAll(AppDBContext _dbContext)
         {
             // var allBooksDomain = _dbContext.Books.ToList ();

             // Get data from database -domain model
             var allBooksDomain = _dbContext.Books;

             //Map domain models to DTOs
             var allBooksDTO = allBooksDomain.Select(Books => new BookWithAuthorAndPublisherDTO() {
                 Id = Books.Id,
                 Title = Books.Title,
                 Description = Books.Description,
                 IsRead = Books.IsRead,
                 DateRead = Books.IsRead ? Books.DateRead.Date : null,
                 Rate = Books.IsRead ? Books.Rate.CompareTo(Books.Rate) : null,
                 Genre = Books.Genre,
                 CoverUrl = Books.CoverUrl,
                 PublisherName = Books.Publishers.Name,
                 AuthorNames = Books.Books_Authors.Select(n => n.Authors.FullName).ToList()

             }).ToList();

             //return DTOs
             return Ok(allBooksDTO);
         }

         [HttpGet]
         [Route("Get-book-by-id/{id}")]
         public IActionResult GetBookById([FromRoute] int id)
         {
             // Get book domain model from database
             var BookWithDomain = _dbContext.Books.Where(n => n.Id == id);
             if (BookWithDomain == null) return NotFound();

             // Map Domain model to DTOs
             var BookWithIdDTO = BookWithDomain.Select(Books => new BookWithAuthorAndPublisherDTO()
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
                 AuthorNames = Books.Books_Authors.Select(n => n.Authors.FullName).ToList()

             });

             return Ok(BookWithIdDTO);
         }

         [HttpPost("Add-book")]
         public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
         {
             // Map DTO to domain model
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

             // Use domain model to create book
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
             return Ok();
         }

         [HttpPut ("update-book-by-id / {id}")]
         public IActionResult UpdateBookById (int id, [FromBody] AddBookRequestDTO BookDTO)
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
                 _dbContext.SaveChanges ();
             }
             var AuthorDomain = _dbContext.Books_Authors.Where(a => a.BookId == id).ToList();
             if(AuthorDomain != null)
             {
                 _dbContext.Books_Authors.RemoveRange(AuthorDomain);
                 _dbContext.SaveChanges();
             }
             foreach(var authorid in BookDTO.AuthorIds)
             {
                 var _book_author = new Books_Authors
                 {
                     BookId = id,
                     AuthorId = authorid
                 };
                 _dbContext.Books_Authors.Add( _book_author );
                 _dbContext.SaveChanges() ;
             }


             return Ok(BookDTO);
         }

         [HttpDelete ("delete-book-by-id / {id}")]
         public IActionResult DeleteBookById (int id)
         {
             var bookDomain = _dbContext.Books.FirstOrDefault(b => b.Id == id);
             if (bookDomain != null)
             {
                 _dbContext.Books.Remove(bookDomain);
                 _dbContext.SaveChanges();
             }
             return Ok();
         }*/

        //use reposity parttern
        [HttpGet("get-all-books")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allbooks = _bookRepository.GetAllBooks(filterOn, filterQuery);
            return Ok(allbooks);
        }

        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookwithidDTO = _bookRepository.GetBookById(id);
            return Ok(bookwithidDTO);
        }

        [HttpPost("add-book")]
        [ValidateModel]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var bookadd = _bookRepository.AddBook(addBookRequestDTO);
            return Ok(bookadd);
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO BookDTO)
        {
            var updatebook = _bookRepository.UpdateBookById(id, BookDTO);
            return Ok(updatebook);
        }

        [HttpDelete ("delete-book-by-id/{id}")]
        public IActionResult DeleteBookById(int id)
        {
            var deletebook = _bookRepository.DeleteBookById(id);
            return Ok(deletebook);
        }
    } 
}
