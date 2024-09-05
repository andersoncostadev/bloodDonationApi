using Application.Interfaces;
using MediatR;

namespace Application.Queries.Donation.GetAll
{
    public class GetAllDonationQueryHandler : IRequestHandler<GetAllDonationQuery, GetAllDonationQueryResponse>
    {
        private readonly IDonationUseCases _donationUseCases;

        public GetAllDonationQueryHandler(IDonationUseCases donationUseCases)
        {
            _donationUseCases = donationUseCases;
        }

        public async Task<GetAllDonationQueryResponse> Handle(GetAllDonationQuery request, CancellationToken cancellationToken)
        {
            var donations = await _donationUseCases.GetDonationsAsync(request.PageNumber, request.PageSize) ?? throw new ApplicationException("Failed to get donations");
            var totalCount = donations.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetAllDonationQueryResponse(donations, request.PageNumber, request.PageSize, totalPages);
        }
    }
}
