namespace Application.Commands.v1.StockBlood
{
    public class UpdateStockBloodCommandResponse
    {
        public UpdateStockBloodCommandResponse(Guid id, string? bloodType, string? rhFactor, int? quantityML)
        {
            Id = id;
            BloodType = bloodType;
            RhFactor = rhFactor;
            QuantityML = quantityML;
        }

        public Guid Id { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public int? QuantityML { get; set; }
    }
}
