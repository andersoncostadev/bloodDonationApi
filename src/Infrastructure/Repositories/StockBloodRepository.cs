using Domain.Entities.v1;
using Domain.Repositories;
using Infrastructure.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    class StockBloodRepository : IStockBloodRepository
    {
        private readonly ApplicationDbContext _context;

        public StockBloodRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StockBloodEntity> AddAsync(StockBloodEntity entity)
        {
            _context.StockBloods!.Add(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.StockBloods!.FindAsync(id);

            if (entity == null)
                return false;

            _context.StockBloods!.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StockBloodEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.StockBloods!
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<StockBloodEntity>> GetBloodStockReportAsync(int pageNumber, int pageSize)
        {
            var bloodStockReport = _context.StockBloods!
                .AsNoTracking()
                .GroupBy(s => new { s.BloodType, s.RhFactor })
                .Select(group => new StockBloodEntity
                {
                    BloodType = group.Key.BloodType,
                    RhFactor = group.Key.RhFactor,
                    QuantityML = group.Sum(s => s.QuantityML)
                })
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return bloodStockReport;
        }

        public async Task<StockBloodEntity?> GetByBloodTypeAndRhFactorAsync(string bloodType, string rhFactor)
        {
            return await _context.StockBloods!
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.BloodType == bloodType && d.RhFactor == rhFactor);
        }

        public async Task<StockBloodEntity> GetByIdAsync(Guid id)
        {
            var stockBlood = await _context.StockBloods!.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

            return stockBlood!;
        }

        public async Task<StockBloodEntity> UpdateAsync(StockBloodEntity entity)
        {
            var stockBlood = _context.StockBloods!.FirstOrDefault(d => d.Id == entity.Id);

            if (stockBlood == null)
                return null;

            _context.Entry(stockBlood).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<StockBloodEntity> UpdateStockBloodAsync(StockBloodEntity stockEntity)
        {
            var stockBlood = await _context.StockBloods!
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.BloodType == stockEntity.BloodType && d.RhFactor == stockEntity.RhFactor);

            if (stockBlood == null)
            {
                await _context.StockBloods!.AddAsync(stockEntity);
            }
            else
            {
                stockBlood.QuantityML += stockEntity.QuantityML;

                _context.Entry(stockBlood).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return stockEntity;
        }

    }
}
