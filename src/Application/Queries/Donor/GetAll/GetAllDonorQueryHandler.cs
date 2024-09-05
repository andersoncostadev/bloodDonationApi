using Application.Interfaces;
using MediatR;

namespace Application.Queries.Donor.GetAll
{
    public class GetAllDonorQueryHandler : IRequestHandler<GetAllDonorQuery, GetAllDonorQueryResponse>
    {
        private readonly IDonorUseCases _donorUseCases;

        public GetAllDonorQueryHandler(IDonorUseCases donorUseCases)
        {
            _donorUseCases = donorUseCases;
        }

        public async Task<GetAllDonorQueryResponse> Handle(GetAllDonorQuery request, CancellationToken cancellationToken)
        {
            var donors = await _donorUseCases.GetDonorsAsync(request.PageNumber, request.PageSize) ?? throw new ApplicationException("Failed to get donors");
            var totalCount = donors.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new GetAllDonorQueryResponse(donors, request.PageNumber,request.PageSize,totalPages);
        }
    }
}
