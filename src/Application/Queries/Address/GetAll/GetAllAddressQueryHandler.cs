using Application.Interfaces;
using MediatR;

namespace Application.Queries.Address.GetAll
{
    public class GetAllAddressQueryHandler : IRequestHandler<GetAllAddressQuery, GetAllAddressQueryResponse>
    {
        private readonly IAddressUseCases _addressuseCases;

        public GetAllAddressQueryHandler(IAddressUseCases useCases)
        {
            _addressuseCases = useCases;
        }

        public async Task<GetAllAddressQueryResponse> Handle(GetAllAddressQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _addressuseCases.GetAddressesAsync(request.PageNumber, request.PageSize) ?? throw new ApplicationException("Failed to get addresses");
            var totalCount = addresses.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetAllAddressQueryResponse(addresses, request.PageNumber, request.PageSize, totalPages);
        }
    }
}
