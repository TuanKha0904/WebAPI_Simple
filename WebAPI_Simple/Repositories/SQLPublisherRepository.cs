using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Simple.Data;
using WebAPI_Simple.Models.Domain;
using WebAPI_Simple.Models.DTO;

namespace WebAPI_Simple.Repositories
{
    public class SQLPublisherRepository : IPublisherRepository
    {
        private readonly AppDBContext? _dbContext;
        public SQLPublisherRepository(AppDBContext DbContext) 
        {
            this._dbContext = DbContext;
        }

        public  List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            // Get data from database - Domain model
            var allPublisher = _dbContext.Publishers.AsQueryable();

            // map domain model to DTOs
            var allPublisherDTO = new List<PublisherDTO>();
            foreach (var item in allPublisher)
            {
                allPublisherDTO.Add(new PublisherDTO()
                {
                       Id = item.Id,
                       Name = item.Name
                });
            }

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace (filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublisher = allPublisher.Where(x => x.Name.Contains(filterQuery));
                }
            }

            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    allPublisher = isAscending?allPublisher.OrderBy(x => x.Name) : allPublisher.OrderByDescending(x => x.Name); 
                }
            }

            //Paging
            var skipResults = (pageNumber - 1) * pageSize;
            return allPublisherDTO.Skip(skipResults).Take(pageSize).ToList();

        }

        public PublisherNoIdDTO GetPublisherById(int id)
        {
            var publisherwithmain =  _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisherwithmain == null)
            {
                return null;
            }
            // map model domain to DTOs
            var publishernoidDTO = new PublisherNoIdDTO
            {
                Name = publisherwithmain.Name
            };
            return publishernoidDTO;
        }

        public AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO)
        {
            var publisherdomainmodel = new Publishers
            {
                Name = addPublisherRequestDTO.Name
            };

            // Use domain model to add Publisher
            _dbContext.Publishers.Add(publisherdomainmodel);
            _dbContext.SaveChanges();
            return addPublisherRequestDTO;

        }

        public PublisherNoIdDTO? UpdatePublisherById(int id, PublisherNoIdDTO publishernoidDTO)
        {
            var publisherdomain = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisherdomain != null)
            {
                publisherdomain.Name = publishernoidDTO.Name;
                _dbContext.SaveChanges();
            }
            return publishernoidDTO;
        }

        public Publishers? DeletePublisherById(int id)
        {
            var publisherdomain = _dbContext.Publishers.FirstOrDefault(p => p.Id == id);
            if (publisherdomain != null)
            {
                _dbContext.Publishers.Remove(publisherdomain);
                _dbContext.SaveChanges();
            }
            return null;
        }
    }
}
