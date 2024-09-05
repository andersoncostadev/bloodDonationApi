using MediatR;

namespace Application.Queries.Donation.GetDonationsReport
{
    public class GetDonationsReportQuery : IRequest<GetDonationsReportQueryResponse>
    {
        public GetDonationsReportQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
