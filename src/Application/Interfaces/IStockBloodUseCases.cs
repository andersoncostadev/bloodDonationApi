using Application.Dtos;

namespace Application.Interfaces
{
    public interface IStockBloodUseCases
    {
        Task<IList<StockBloodDto>> GetBloodStockReportAsync(int pageNumber, int pageSize);
        Task<StockBloodDto?> GetByBloodTypeAndRhFactorAsync(string bloodType, string rhFactor);
        Task<StockBloodDto> UpdateStockBloodAsync(StockBloodDto stockBlood);
    }
}
