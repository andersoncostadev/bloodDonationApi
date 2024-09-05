namespace Infrastructure.Exceptions
{
    public class InvalidPostalCodeException(string postalCode) : 
        Exception(message: $"The postal code '{postalCode}' is invalid.")
    {
    }
}
