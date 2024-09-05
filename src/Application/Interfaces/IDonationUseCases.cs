using Application.Dtos;

namespace Application.Interfaces
{
    public interface IDonationUseCases
    {
        Task<DonationDto> CreateDonationAsync(DonationDto donation);
        Task<IEnumerable<DonationDto>> GetDonationsAsync(int pageSize, int pageNumber);
        Task<DonationDto> GetDonationByIdAsync(Guid id);
        Task<IList<DonationDto>> GetDonationsReportAsync(int pageNumber, int pageSize);
        Task<DonationDto> UpdateDonationAsync(DonationDto donation);
    }
}
