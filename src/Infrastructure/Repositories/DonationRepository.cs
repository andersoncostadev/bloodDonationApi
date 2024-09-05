using Domain.Entities.v1;
using Domain.Repositories;
using Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DonationRepository : IDonationRepository
    {
        private readonly ApplicationDbContext _context;

        public DonationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonationEntity> AddAsync(DonationEntity entity)
        {
            _context.Donations!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
           var entity = await _context.Donations!.FindAsync(id);

            if (entity == null)
                return false;

            _context.Donations!.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DonationEntity>> GetAllAsync(int pageSize, int pageNumber)
        {
           return await _context.Donations!
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DonationEntity> GetByIdAsync(Guid id)
        {
            var donation = await _context.Donations!.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            return donation!;
        }

        public Task<List<DonationEntity>> GetDonationsReportAsync(int pageNumber, int pageSize)
        {
            var today = DateTime.UtcNow.Date;

            var thirtyDaysAgo = today.AddDays(-30);

            var donations = _context.Donations!
                .AsNoTracking()
                .Where(d => d.DonationDate >= thirtyDaysAgo && d.DonationDate <= today)
                .Include(d => d.Donor)
                .OrderByDescending(d => d.DonationDate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return donations;
        }

        public async Task<DonationEntity> UpdateAsync(DonationEntity entity)
        {
            var donation = await _context.Donations!
                .FirstOrDefaultAsync(d => d.DonorId == entity.DonorId);

            if (donation == null)
                return null;

            _context.Entry(donation).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
