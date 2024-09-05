using Application.Dtos;

namespace Application.Queries.Address.GetAll
{
    public class GetAllAddressQueryResponse
    {
        public GetAllAddressQueryResponse(IEnumerable<AddressDto>? addresses, int pageNumber, int pageSize, int totalPages)
        {
            Addresses = addresses;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        public IEnumerable<AddressDto>? Addresses { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
