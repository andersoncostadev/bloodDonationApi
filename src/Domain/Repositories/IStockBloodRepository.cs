using Domain.Entities.v1;

namespace Domain.Repositories
{
    public interface IStockBloodRepository : IRepository<StockBloodEntity>
    {
        Task<StockBloodEntity> UpdateStockBloodAsync(StockBloodEntity stockEntity);

        Task<StockBloodEntity?> GetByBloodTypeAndRhFactorAsync(string bloodType, string rhFactor);

        Task<List<StockBloodEntity>> GetBloodStockReportAsync(int pageNumber, int pageSize);   
    }
}
