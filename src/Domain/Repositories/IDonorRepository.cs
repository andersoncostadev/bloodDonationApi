using Domain.Entities.v1;

namespace Domain.Repositories
{
    public interface IDonorRepository : IRepository<DonorEntity>
    {
        Task<bool> EmailExistsAsync(string email);
        Task<DonorEntity> GetByFullName(string name);
    }
}
