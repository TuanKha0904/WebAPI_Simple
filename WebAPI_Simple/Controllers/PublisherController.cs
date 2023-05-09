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
    public class PublisherController : ControllerBase
    {
        private readonly AppDBContext? _dbContext;
        private readonly IPublisherRepository? _publisherRepository;

        public PublisherController(AppDBContext? dbContext, IPublisherRepository? publisherRepository)
        {
            _dbContext = dbContext;
            _publisherRepository = publisherRepository;
        }

        // Use repository parttern
        [HttpGet("get-all-publisher")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allpublisher = _publisherRepository.GetAllPublishers(filterOn, filterQuery);
            return Ok(allpublisher);
        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById([FromRoute] int id) 
        { 
            var publisherwithidDTO = _publisherRepository.GetPublisherById(id);
            return Ok(publisherwithidDTO);
        }

        [HttpPost("add-publisher")]
        [ValidateModel]
        public IActionResult AddPublisher([FromBody] AddPublisherRequestDTO addpublisherRequestDTO)
        {
            var publisheradd = _publisherRepository.AddPublisher(addpublisherRequestDTO);
            return Ok(publisheradd);
        }

        [HttpPut("update-publisher-by-id/{id}")]
        public IActionResult UpdatePublisherById(int id, [FromBody] PublisherNoIdDTO PublisherDTO)
        {
            var updatepublisher = _publisherRepository.UpdatePublisherById(id, PublisherDTO);
            return Ok(updatepublisher);
        }

        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePublisherById(int id)
        {
            var deletepublisher = _publisherRepository.DeletePublisherById(id);
            return Ok(deletepublisher);
        }

    }
}
