using Application.Dtos;

namespace Application.Interfaces
{
    public interface IAddressUseCases
    {
        Task<IEnumerable<AddressDto>> GetAddressesAsync(int pageNumber, int pageSize);
        Task<AddressDto> GetByPostalCodeAsync(string postalCode);
    }
}
