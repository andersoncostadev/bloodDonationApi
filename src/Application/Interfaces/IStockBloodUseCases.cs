using Application.Dtos;

namespace Application.Interfaces
{
    public interface IStockBloodUseCases
    {
        Task<IList<StockBloodDto>> GetBloodStockReportAsync(int pageNumber, int pageSize);
        Task<StockBloodDto> UpdateStockBloodAsync(StockBloodDto stockBlood);
    }
}
