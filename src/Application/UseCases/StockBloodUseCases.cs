using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.v1;
using Domain.Events;
using Domain.Repositories;
using Domain.Services;

namespace Application.UseCases
{
    public class StockBloodUseCases : IStockBloodUseCases
    {
        private readonly IStockBloodRepository _stockBloodRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly IMapper _mapper;

        public StockBloodUseCases(IStockBloodRepository stockBloodRepository, IDomainEventDispatcher domainEventDispatcher, IMapper mapper)
        {
            _stockBloodRepository = stockBloodRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _mapper = mapper;
        }

        public async Task<IList<StockBloodDto>> GetBloodStockReportAsync(int pageNumber, int pageSize)
        {
            var bloodStockReport = await _stockBloodRepository.GetBloodStockReportAsync(pageNumber, pageSize);

            return bloodStockReport?.Select(stockBlood => _mapper.Map<StockBloodDto>(stockBlood)).ToList() ?? [];
        }

        public async Task<StockBloodDto?> GetByBloodTypeAndRhFactorAsync(string bloodType, string rhFactor)
        {
            var stockBlood = await _stockBloodRepository.GetByBloodTypeAndRhFactorAsync(bloodType, rhFactor);

            return stockBlood == null ? null : _mapper.Map<StockBloodDto>(stockBlood);
        }

        public async Task<StockBloodDto> UpdateStockBloodAsync(StockBloodDto stockBlood, bool isUpdate = false, int quantityDifference = 0)
        {
            var existingStock = await _stockBloodRepository.GetByBloodTypeAndRhFactorAsync(stockBlood.BloodType!, stockBlood.RhFactor!);

            int minimumStock = 420;

            if (existingStock == null)
            {
                var newStock = _mapper.Map<StockBloodEntity>(stockBlood);

                if(newStock.QuantityML <= minimumStock)
                {
                    var bloodStockLowEvent = new BloodStockLowEvent(newStock.BloodType, newStock.RhFactor, newStock.QuantityML.Value);
                    await _domainEventDispatcher.Dispatch(bloodStockLowEvent);
                }

                var createdStock = await _stockBloodRepository.AddAsync(newStock);

                return _mapper.Map<StockBloodDto>(createdStock);
            }
            else
            {
                bool wasAboveMinimum = existingStock.QuantityML > minimumStock;

                var updateStock = await _stockBloodRepository.UpdateStockBloodAsync(
                    existingStock, isUpdate: isUpdate, quantityDifference: quantityDifference);

                if (wasAboveMinimum && updateStock.QuantityML <= minimumStock)
                {
                    var bloodStockLowEvent = new BloodStockLowEvent(updateStock.BloodType, updateStock.RhFactor, updateStock.QuantityML.Value);
                    await _domainEventDispatcher.Dispatch(bloodStockLowEvent);
                }

                return _mapper.Map<StockBloodDto>(updateStock);
            }
        }
    }
}
