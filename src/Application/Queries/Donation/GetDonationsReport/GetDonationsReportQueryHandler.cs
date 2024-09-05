using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Queries.Donation.GetDonationsReport
{
    public class GetDonationsReportQueryHandler : IRequestHandler<GetDonationsReportQuery, GetDonationsReportQueryResponse>
    {
        private readonly IDonationUseCases _donationUseCases;
        private readonly IDonorUseCases _donorUseCases;
        private readonly IMapper _mapper;

        public GetDonationsReportQueryHandler(IDonationUseCases donationUseCases, IDonorUseCases donorUseCases, IMapper mapper)
        {
            _donationUseCases = donationUseCases;
            _donorUseCases = donorUseCases;
            _mapper = mapper;
        }

        public async Task<GetDonationsReportQueryResponse> Handle(GetDonationsReportQuery request, CancellationToken cancellationToken)
        {
            var donations = await _donationUseCases.GetDonationsReportAsync(request.PageNumber, request.PageSize) ?? throw new ApplicationException("Failed to get donations report");

            var totalCount = donations.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var donationReport = new List<DonationReportDto>();

            foreach (var donation in donations)
            {
                var donor = await _donorUseCases.GetDonorByIdAsync(donation.DonorId) ?? throw new DonorNotFoundException(donation.DonorId);

                var donationReportDto = _mapper.Map<DonationReportDto>(donation);

                if (donor != null)
                {
                    donationReportDto.DonorFullName = donor.FullName;
                    donationReportDto.BloodType = donor.BloodType;
                    donationReportDto.RhFactor = donor.RhFactor;
                }

                donationReport.Add(donationReportDto);
            }

            return new GetDonationsReportQueryResponse(donationReport, donationReport.Count, request.PageNumber, request.PageSize, totalPages);
        }
    }
}
