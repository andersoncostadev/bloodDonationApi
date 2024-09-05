using Application.Dtos;

namespace Application.Queries.Donor.GetAll
{
    public class GetAllDonorQueryResponse
    {
        public GetAllDonorQueryResponse(IEnumerable<DonorDto>? donors, int pageNumber, int pageSize, int totalPages)
        {
            Donors = donors;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IEnumerable<DonorDto>? Donors { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
