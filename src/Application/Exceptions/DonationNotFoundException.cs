namespace Application.Exceptions
{
    public class DonationNotFoundException(Guid id) : Exception($"Donation with id {id} is not found")
    {
    }
}
