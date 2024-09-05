using Application.Dtos;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.v1;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Exceptions;

namespace Application.UseCases
{
    public class DonorUseCases : IDonorUseCases
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPostalCodeService _postalCodeService;
        private readonly IMapper _mapper;

        public DonorUseCases(IDonorRepository donorRepository, IAddressRepository addressRepository, IPostalCodeService postalCodeService, IMapper mapper)
        {
            _donorRepository = donorRepository;
            _addressRepository = addressRepository;
            _postalCodeService = postalCodeService;
            _mapper = mapper;
        }

        public async Task<DonorDto> CreateDonorAsync(DonorDto donor)
        {
            var getAddressByPostal = await _postalCodeService.GetAddressByPostalCodeAsync(donor.Address!.ZipCode!)
                ?? throw new InvalidPostalCodeException(donor.Address!.ZipCode!);

            var donorEntity = _mapper.Map<DonorEntity>(donor);

            var addressEntity = new AddressEntity
            {
                Street = donor.Address.Street,
                City = getAddressByPostal.City,
                State = getAddressByPostal.State,
                ZipCode = getAddressByPostal.ZipCode,
                Number = donor.Address.Number,
                Neighborhood = donor.Address.Neighborhood,
                DonorId = donorEntity.Id
            };

            var createdAddress = await _addressRepository.AddAsync(addressEntity);

            donorEntity.Address = createdAddress;

            var createdDonor = await _donorRepository.AddAsync(donorEntity);

            return _mapper.Map<DonorDto>(createdDonor);
        }

        public async Task<bool> DeleteDonorAsync(Guid id)
        {
            var donorEntity = await _donorRepository.GetByIdAsync(id)
                ?? throw new DonorNotFoundException(id);

            if(donorEntity.Address != null)
                await _addressRepository.DeleteAsync(donorEntity.Id);

            await _donorRepository.DeleteAsync(id);

            return true;
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _donorRepository.EmailExistsAsync(email);
        }

        public async Task<DonorDto> GetDonorByIdAsync(Guid id)
        {
            var donorEntity = await _donorRepository.GetByIdAsync(id);

            return donorEntity == null ? throw new DonorNotFoundException(id) : _mapper.Map<DonorDto>(donorEntity);
        }

        public async Task<DonorDto> GetDonorFullNameAsync(string name)
        {
            var donorEntity = await _donorRepository.GetByFullName(name);

            return donorEntity == null ? throw new DonorNotFoundException(name) : _mapper.Map<DonorDto>(donorEntity);
        }

        public async Task<IEnumerable<DonorDto>> GetDonorsAsync(int pageNumnber, int pageSize)
        {
            var donors = await _donorRepository.GetAllAsync(pageNumnber, pageSize);

            var donorsPage = donors.Select(donor => _mapper.Map<DonorDto>(donor)).ToList();

            return donorsPage;
        }

        public async Task<DonorDto> UpdateDonorAsync(DonorDto donor)
        {
            var donorEntity = await _donorRepository.GetByIdAsync(donor.Id)
                ?? throw new DonorNotFoundException(donor.Id);

            if(donor.Address != null)
            {
                var addressEntity = await _addressRepository.GetByIdAsync(donor.Address.Id);

                if (addressEntity != null)
                {
                    _mapper.Map(donor.Address, addressEntity);
                    addressEntity.DonorId = donorEntity.Id;

                    donorEntity.Address = await _addressRepository.UpdateAsync(addressEntity);
                }
                else 
                {
                    var newAddressEntity = _mapper.Map<AddressEntity>(donor.Address);
                    newAddressEntity.DonorId = donorEntity.Id;

                    donorEntity.Address = await _addressRepository.AddAsync(newAddressEntity);
                }
            }

            _mapper.Map(donor, donorEntity);

            var updatedDonor = await _donorRepository.UpdateAsync(donorEntity);

            return _mapper.Map<DonorDto>(updatedDonor);
        }

    }
}
