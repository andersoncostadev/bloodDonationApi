using Application.Dtos;

namespace Application.Queries.Donation.GetAll
{
    public class GetAllDonationQueryResponse
    {
        public GetAllDonationQueryResponse(IEnumerable<DonationDto>? donations, int pageNumber, int pageSize, int totalPages)
        {
            Donations = donations;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IEnumerable<DonationDto>? Donations { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
