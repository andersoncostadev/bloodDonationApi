using Domain.Entities.v1;
using Domain.Repositories;
using Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddressEntity> AddAsync(AddressEntity entity)
        {
            var existingAddress = await _context.Addresses!
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.DonorId == entity.DonorId);

            if (existingAddress != null)
            {
                var trackedEntity = _context.ChangeTracker
                    .Entries<AddressEntity>()
                    .FirstOrDefault(e => e.Entity.DonorId == entity.DonorId);

                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity.Entity).CurrentValues.SetValues(entity);
                }
                else
                {
                    _context.Entry(entity).State = EntityState.Modified;
                }
            }
            else
            {
                _context.Addresses!.Add(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Addresses!.FirstOrDefaultAsync(d => d.DonorId == id);

            if (entity == null)
                return false;

            _context.Addresses!.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<AddressEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Addresses!
                .OrderBy(d => d.Street)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AddressEntity> GetByIdAsync(Guid id)
        {
            var address = await _context.Addresses!
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return address!;
        }

        public async Task<AddressEntity> UpdateAsync(AddressEntity entity)
        {
            var address = await _context.Addresses!
                .FirstOrDefaultAsync(a => a.DonorId == entity.DonorId);

            if (address == null)
                return null;

            _context.Entry(address).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
