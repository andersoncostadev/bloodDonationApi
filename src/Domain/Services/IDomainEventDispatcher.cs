namespace Domain.Services
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch<T>(T domainevent) where T : class;
    }
}
