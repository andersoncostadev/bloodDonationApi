namespace Application.Exceptions
{
    public class DonorNotFoundException : Exception
    {
        public DonorNotFoundException(string name)
            : base($"Donor with name '{name}' is not found")
        {
        }

        public DonorNotFoundException(Guid id)
            : base($"Donor with id '{id}' is not found")
        {
        }
    }
}
