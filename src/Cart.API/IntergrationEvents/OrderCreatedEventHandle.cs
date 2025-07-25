namespace Cart.API.IntergrationEvents;
public class OrderCreatedEventHandle(ICartService cartService, ILogger<OrderCreatedEventHandle> logger) : IIntegrationEventHandler<OrderCreatedEvent>
{
    private readonly ICartService _cartService = cartService;
    private readonly ILogger<OrderCreatedEventHandle> _logger = logger;

    public async Task Handle(OrderCreatedEvent @event)
    {
        _logger.LogInformation("Handling OrderCreatedEvent for UserId: {UserId}", @event.UserId);
        var cart = await _cartService.GetCartAsync(@event.UserId.ToString());
        if (cart != null)
        {
            cart.Items.Clear();
            await _cartService.UpdateCartAsync(cart);
            _logger.LogInformation("Cart cleared for UserId: {UserId}", @event.UserId);
        }
    }
}