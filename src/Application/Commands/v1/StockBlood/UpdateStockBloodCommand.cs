using MediatR;

namespace Application.Commands.v1.StockBlood
{
    public class UpdateStockBloodCommand : IRequest<UpdateStockBloodCommandResponse>
    {
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public int QuantityML { get; set; }
    }
}
