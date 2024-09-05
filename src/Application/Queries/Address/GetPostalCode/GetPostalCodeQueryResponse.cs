namespace Application.Queries.Address.GetPostalCode
{
    public class GetPostalCodeQueryResponse
    {
        public GetPostalCodeQueryResponse(string? street, string? neighborhood, string? city, string? state, string? zipCode)
        {
            Street = street;
            Neighborhood = neighborhood;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public string? Street { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}
