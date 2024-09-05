using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Exceptions;

namespace Application.UseCases
{
    public class AddressUseCases : IAddressUseCases
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IPostalCodeService _postalCodeService;
        private readonly IMapper _mapper;

        public AddressUseCases(IAddressRepository addressRepository, IPostalCodeService postalCodeService,IMapper mapper)
        {
            _addressRepository = addressRepository;
            _postalCodeService = postalCodeService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> GetAddressesAsync(int pageNumber, int pageSize)
        {
            var addresses = await _addressRepository.GetAllAsync(pageNumber, pageSize);

            var addressesPage = addresses.Select(address => _mapper.Map<AddressDto>(address)).ToList();

            return addressesPage;
        }

        public async Task<AddressDto> GetByPostalCodeAsync(string postalCode)
        {
            var getPostalCode = await _postalCodeService.GetAddressByPostalCodeAsync(postalCode);

            if (getPostalCode == null)
                throw new InvalidPostalCodeException(postalCode);

            return _mapper.Map<AddressDto>(getPostalCode);
        }
    }
}
