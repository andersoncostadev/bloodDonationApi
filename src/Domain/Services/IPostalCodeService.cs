using Domain.Entities.v1;

namespace Domain.Services
{
    public interface IPostalCodeService
    {
        Task<AddressEntity> GetAddressByPostalCodeAsync(string postalCode);
    }
}
