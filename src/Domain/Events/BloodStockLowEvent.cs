namespace Domain.Events
{
    public class BloodStockLowEvent
    {
        public BloodStockLowEvent(string? bloodType, string? rhFactor, int currentQuantity)
        {
            BloodType = bloodType;
            RhFactor = rhFactor;
            CurrentQuantity = currentQuantity;
        }

        public string? BloodType { get; set; }
        public string? RhFactor { get; set; }
        public int CurrentQuantity { get; set; }
    }
}
