using MediatR;

namespace Application.Queries.Donation.GetAll
{
    public class GetAllDonationQuery : IRequest<GetAllDonationQueryResponse>
    {
        public GetAllDonationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
