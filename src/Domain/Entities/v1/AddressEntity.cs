namespace Domain.Entities.v1
{
    public class AddressEntity : BaseEntity
    {
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public Guid DonorId { get; set; }
        public DonorEntity? Donor { get; set; }
    }
}
