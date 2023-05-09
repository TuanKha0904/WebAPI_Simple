using WebAPI_Simple.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI_Simple.Models.Domain;
using WebAPI_Simple.Models.DTO;

namespace WebAPI_Simple.Repositories
{
    public class SQLAuthorRepository : IAuthorRepository
    {
        private readonly AppDBContext? _dbContext;
        public SQLAuthorRepository (AppDBContext dbContext)
        {
            _dbContext = dbContext; 
        }

        public List<AuthorDTO> GetAllAuthors(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            //Get data from database - Domain model
            var allAuthorDomain = _dbContext.Authors.AsQueryable();
            //map domain model to DTOs
            var allAuthorDTO = new List<AuthorDTO>();
            foreach (var author in allAuthorDomain)
            {
                allAuthorDTO.Add(new AuthorDTO()
                {
                    Id = author.Id,
                    Fullname = author.Fullname
                });
            }
            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Fullname", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthorDomain = allAuthorDomain.Where(x => x.Fullname.Contains(filterQuery));
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("fullname", StringComparison.OrdinalIgnoreCase))
                {
                    allAuthorDomain = isAscending ? allAuthorDomain.OrderBy(x => x.Fullname) : allAuthorDomain.OrderByDescending(x => x.Fullname);
                }
            }
            //Paging
            var skipResults = (pageNumber - 1) * pageSize;
            //return DTOs
            return allAuthorDTO.Skip(skipResults).Take(pageSize).ToList();
        }

        public AuthorNoIdDTO GetAuthorById(int id)
        {
            //get author domain model from databse
            var authorwithidmain = _dbContext.Authors.FirstOrDefault(x => x.Id == id);
            if (authorwithidmain == null)
            {
                return null;
            }
            //map domain model to DTOs
            var authorNoIdDTO = new AuthorNoIdDTO
            {
                Fullname = authorwithidmain.Fullname
            };
            return authorNoIdDTO;
        }

        public AddAuthorRequestDTO AddAuthor(AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var AuthorDomainModel = new Authors
            {
                Fullname = addAuthorRequestDTO.FullName
            };

            //Use domain model to add Author
            _dbContext.Authors.Add(AuthorDomainModel);
            _dbContext.SaveChanges();
            return addAuthorRequestDTO;
        }

        public AuthorNoIdDTO? UpdateAuthorById(int id, AuthorNoIdDTO AuthornoidDTO)
        {
            var authordomain = _dbContext.Authors.FirstOrDefault(n => n.Id == id);
            if (authordomain != null)
            {
                authordomain.Fullname = AuthornoidDTO.Fullname;
                _dbContext.SaveChanges();
            }
            return AuthornoidDTO;

        }

        public Authors? DeleteAuthorById(int id)
        {
            var authorDomain = _dbContext.Authors.FirstOrDefault(a => a.Id == id);
            if (authorDomain != null)
            {
                _dbContext.Authors.Remove(authorDomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
    }
}
