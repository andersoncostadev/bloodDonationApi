namespace Application.Exceptions
{
    public class AddressNotFoundException(Guid id) : Exception($"Address with id {id} is not found")
    {
    }
}
