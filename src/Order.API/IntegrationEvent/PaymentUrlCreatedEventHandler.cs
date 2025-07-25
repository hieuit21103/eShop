using StackExchange.Redis;

namespace Order.API.IntegrationEvent;

public class PaymentUrlCreatedEventHandler(ILogger<PaymentUrlCreatedEventHandler> logger, IOrderService orderService) : IIntegrationEventHandler<PaymentUrlCreatedEvent>
{
    private readonly ILogger<PaymentUrlCreatedEventHandler> _logger = logger;
    private readonly IOrderService _orderService = orderService;

    public async Task Handle(PaymentUrlCreatedEvent @event)
    {
        // Side Effets: Will add later
    }
}