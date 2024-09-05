using Application.Dtos;

namespace Application.Interfaces
{
    public interface IDonorUseCases
    {
        Task<DonorDto> CreateDonorAsync(DonorDto donor);
        Task<DonorDto> GetDonorByIdAsync(Guid id);
        Task<IEnumerable<DonorDto>> GetDonorsAsync(int pageSize, int pageNumber);
        Task<DonorDto> GetDonorFullNameAsync(string name);
        Task<DonorDto> UpdateDonorAsync(DonorDto donor);
        Task<bool> DeleteDonorAsync(Guid id);
        Task<bool> EmailExistsAsync(string email);
    }
}
