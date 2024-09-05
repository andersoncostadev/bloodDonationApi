using MediatR;

namespace Application.Queries.Address.GetAll
{
    public class GetAllAddressQuery : IRequest<GetAllAddressQueryResponse>
    {
        public GetAllAddressQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
