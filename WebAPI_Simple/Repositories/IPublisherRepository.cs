using WebAPI_Simple.Models.Domain;
using WebAPI_Simple.Models.DTO;

namespace WebAPI_Simple.Repositories
{
    public interface IPublisherRepository
    {
        List<PublisherDTO> GetAllPublishers(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100);
        PublisherNoIdDTO GetPublisherById(int id);
        AddPublisherRequestDTO AddPublisher(AddPublisherRequestDTO addPublisherRequestDTO);
        PublisherNoIdDTO? UpdatePublisherById (int id, PublisherNoIdDTO publishernoidDTO);
        Publishers? DeletePublisherById (int id);
    }
}
