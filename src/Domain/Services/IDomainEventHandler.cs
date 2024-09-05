namespace Domain.Services
{
    public interface IDomainEventHandler<T>
    {
        Task Handle(T domainEvent);
    }
}
