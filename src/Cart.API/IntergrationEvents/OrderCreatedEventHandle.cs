namespace Cart.API.IntergrationEvents;
public class OrderCreatedEventHandle : IIntegrationEventHandler<OrderCreatedEvent>
{
    private readonly ICartService _cartService;
    private readonly ILogger<OrderCreatedEventHandle> _logger;

    public OrderCreatedEventHandle(ICartService cartService, ILogger<OrderCreatedEventHandle> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OrderCreatedEvent for UserId: {UserId}", @event.UserId);
        var cart = await _cartService.GetCartAsync(@event.UserId);
        if (cart != null)
        {
            cart.Items.Clear();
            await _cartService.UpdateCartAsync(cart);
            _logger.LogInformation("Cart cleared for UserId: {UserId}", @event.UserId);
        }
    }
}