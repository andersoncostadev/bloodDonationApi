using Application.Interfaces;
using MediatR;

namespace Application.Commands.v1.StockBlood
{
    public class UpdateStockBloodCommandHandler : IRequestHandler<UpdateStockBloodCommand, UpdateStockBloodCommandResponse>
    {
        private readonly IStockBloodUseCases _stockBloodUseCases;

        public UpdateStockBloodCommandHandler(IStockBloodUseCases stockBloodUseCases)
        {
            _stockBloodUseCases = stockBloodUseCases;
        }

        public async Task<UpdateStockBloodCommandResponse> Handle(UpdateStockBloodCommand request, CancellationToken cancellationToken)
        {
            var stockBlood = await _stockBloodUseCases.GetByBloodTypeAndRhFactorAsync(request.BloodType!, request.RhFactor!)
                ?? throw new ApplicationException($"No stock found for BloodType {request.BloodType!} and RhFactor {request.RhFactor!}");

            stockBlood.QuantityML = request.QuantityML;

            var updatedStock = await _stockBloodUseCases.AdjustStockQuantityAsync(stockBlood);

            return new UpdateStockBloodCommandResponse(
                updatedStock.Id,
                updatedStock.BloodType,
                updatedStock.RhFactor,
                updatedStock.QuantityML
            );
        }
    }
}