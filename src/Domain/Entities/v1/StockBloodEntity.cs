namespace Domain.Entities.v1
{
    public class StockBloodEntity :BaseEntity
    {
        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public int? QuantityML { get; set; }
    }
}
