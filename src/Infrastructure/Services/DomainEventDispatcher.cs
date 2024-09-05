using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Dispatch<T>(T domainevent) where T : class
        {
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<T>>();
            foreach (var handler in handlers)
            {
               await handler.Handle(domainevent);
            }
        }
    }
}
