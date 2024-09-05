using MediatR;

namespace Application.Queries.Donor.GetAll
{
    public class GetAllDonorQuery : IRequest<GetAllDonorQueryResponse>
    {
        public GetAllDonorQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
