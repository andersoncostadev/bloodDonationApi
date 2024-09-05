using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.v1;
using Domain.Repositories;

namespace Application.UseCases
{
    public class DonationUseCases : IDonationUseCases
    {
        private readonly IDonationRepository _donationRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IMapper _mapper;

        public DonationUseCases(IDonationRepository donationRepository, IDonorRepository donorRepository,IMapper mapper)
        {
            _donationRepository = donationRepository;
            _donorRepository = donorRepository;
            _mapper = mapper;
        }

        public async Task<DonationDto> CreateDonationAsync(DonationDto donation)
        {
            var donationEntity = _mapper.Map<DonationEntity>(donation);

            var createdDonation = await _donationRepository.AddAsync(donationEntity);

            var donor = await _donorRepository.GetByIdAsync(donation.DonorId) ?? throw new DonorNotFoundException(donation.Id);

            donor.Donations!.Add(createdDonation);

            await _donorRepository.UpdateAsync(donor);

            return _mapper.Map<DonationDto>(createdDonation);
        }

        public async Task<DonationDto> GetDonationByIdAsync(Guid id)
        {
            var donationEntity = await _donationRepository.GetByIdAsync(id);

            return donationEntity == null ? throw new DonationNotFoundException(id) : _mapper.Map<DonationDto>(donationEntity);
        }

        public async Task<IEnumerable<DonationDto>> GetDonationsAsync(int pageSize, int pageNumber)
        {
           var donations = await _donationRepository.GetAllAsync(pageSize, pageNumber);

           var donationsPage = donations.Select(donation => _mapper.Map<DonationDto>(donation)).ToList();

            return donationsPage;
        }

        public async Task<IList<DonationDto>> GetDonationsReportAsync(int pageNumber, int pageSize)
        {
            var donations = await _donationRepository.GetDonationsReportAsync(pageNumber, pageSize);

            return donations?.Select(donation => _mapper.Map<DonationDto>(donation)).ToList() ?? [];
        }

        public async Task<DonationDto> UpdateDonationAsync(DonationDto donation)
        {
            var donationEntity = _mapper.Map<DonationEntity>(donation);

            var updatedDonation = await _donationRepository.UpdateAsync(donationEntity);

            var donor = await _donorRepository.GetByIdAsync(donation.DonorId) ?? throw new DonorNotFoundException(donation.Id);

            var donationToUpdate = donor.Donations!.FirstOrDefault(d => d.Id == donation.Id) ?? throw new DonorNotFoundException(donation.Id);
            donationToUpdate.DonationDate = updatedDonation.DonationDate;
            donationToUpdate.QuantityML = updatedDonation.QuantityML;

            await _donorRepository.UpdateAsync(donor);

            return _mapper.Map<DonationDto>(updatedDonation);
        }
    }
}
