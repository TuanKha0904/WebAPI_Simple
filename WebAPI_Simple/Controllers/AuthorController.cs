using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Simple.Data;
using WebAPI_Simple.Repositories;
using WebAPI_Simple.Models.DTO;
using WebAPI_Simple.Models.Domain;
    using System.Reflection.Metadata.Ecma335;
using WebAPI_Simple.CustomActionFilter;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI_Simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly AppDBContext? _dbContext;
        private readonly IAuthorRepository? _authorRepository;

        public AuthorController (AppDBContext? dbContext, IAuthorRepository? authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }
        //use reposity parttern
        [HttpGet("get-all-authors")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allauthors = _authorRepository.GetAllAuthors(filterOn, filterQuery);
            return Ok(allauthors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById([FromRoute] int id)
        {
            var authorwithidDTO = _authorRepository.GetAuthorById(id);
            return Ok(authorwithidDTO);
        }

        [HttpPost("add-author")]
        [ValidateModel]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            var authoradd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authoradd);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO AuthornoidDTO)
        {
            var updateauthor = _authorRepository.UpdateAuthorById(id, AuthornoidDTO);
            return Ok(updateauthor);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var deleteauthor = _authorRepository.DeleteAuthorById(id);
            return Ok();
        }
    }
}
