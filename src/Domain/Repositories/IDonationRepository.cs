using Domain.Entities.v1;

namespace Domain.Repositories
{
    public interface IDonationRepository : IRepository<DonationEntity>
    {
        Task<List<DonationEntity>> GetDonationsReportAsync(int pageNumber, int pageSize);
    }
}
