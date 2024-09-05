using Application.Dtos;

namespace Application.Queries.Donation.GetDonationsReport
{
    public class GetDonationsReportQueryResponse 
    {
        public GetDonationsReportQueryResponse(IList<DonationReportDto> donations, int totalDonations, int pageNumber, int pageSize, int totalPages)
        {
            Donations = donations;
            TotalDonations = totalDonations;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IList<DonationReportDto> Donations { get; set; }
        public int TotalDonations { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
