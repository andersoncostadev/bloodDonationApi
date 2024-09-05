namespace Application.Dtos
{
    public class StockBloodDto
    {
        public Guid Id { get; set; }
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public int? QuantityML { get; set; }
    }
}
