using Domain.Entities.v1;
using Domain.Repositories;
using Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly ApplicationDbContext _context;

        public DonorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DonorEntity> AddAsync(DonorEntity entity)
        {
            _context.Donors!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Donors!.FindAsync(id);

            if (entity == null)
                return false;

             _context.Donors!.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Donors!.AnyAsync(d => d.Email == email);
        }

        public async Task<IEnumerable<DonorEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Donors!
                .Include(d => d.Address)
                .Include(d => d.Donations)
                .OrderBy(d => d.FullName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<DonorEntity> GetByFullName(string name)
        {
           var donor = await _context.Donors!
                .Include(d => d.Address)
                .Include(d => d.Donations)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.FullName == name);

            return donor!;
        }

        public async Task<DonorEntity> GetByIdAsync(Guid id)
        {
            var donor = await _context.Donors!
                .Include(d => d.Donations)
                .Include(d => d.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return donor!;
        }

        public async Task<DonorEntity> UpdateAsync(DonorEntity entity)
        {
            var donor = await _context.Donors!.FindAsync(entity.Id);

            if (donor == null)
                return null;

            _context.Entry(donor).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

    }
}
