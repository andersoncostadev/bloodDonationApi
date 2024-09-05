using Domain.Events;
using Domain.Services;

namespace Infrastructure.Services
{
    public class BloodStockLowEventHandler : IDomainEventHandler<BloodStockLowEvent>
    {
        private readonly IEmailService _emailService;

        public BloodStockLowEventHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task Handle(BloodStockLowEvent domainEvent)
        {
            var subject = "Aviso: Estoque de sangue baixo";

            var message = $"O estoque de sangue do tipo {domainEvent.BloodType} com fator {domainEvent.RhFactor} está abaixo do mínimo com {domainEvent.CurrentQuantity} mL.";

            await _emailService.SendEmailAsync("andersonmtb88@gmail.com", subject, message);
        }
    }
}
