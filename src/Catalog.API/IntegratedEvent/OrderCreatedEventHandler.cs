namespace Catalog.API.IntegratedEvent;

public class OrderCreatedEventHandler : IIntegrationEventHandler<OrderCreatedEvent>
{
    private readonly IGenericService<Product> _genericService;
    private readonly ILogger<OrderCreatedEventHandler> _logger;

    public OrderCreatedEventHandler(IGenericService<Product> genericService, ILogger<OrderCreatedEventHandler> logger)
    {
        _genericService = genericService;
        _logger = logger;
    }

    public async Task Handle(OrderCreatedEvent @event)
    {
        _logger.LogInformation("Processing OrderCreated event for Order {OrderId}", @event.OrderId);

        try
        {
            foreach (var item in @event.Items)
            {
                var product = await _genericService.GetByIdAsync(item.ProductId);
                product.StockQuantity -= item.Quantity;
                var success = await _genericService.UpdateAsync(product);
                
                if (success == null)
                {
                    _logger.LogError("Failed to reduce stock for Product {ProductId}, Quantity {Quantity}", 
                        item.ProductId, item.Quantity);
                    continue;
                }

                _logger.LogInformation("Successfully reduced stock for Product {ProductId} by {Quantity}", 
                    item.ProductId, item.Quantity);
            }

            _logger.LogInformation("Completed processing OrderCreated event for Order {OrderId}", @event.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing OrderCreated event for Order {OrderId}", @event.OrderId);
            throw; // Re-throw để message được requeue
        }
    }
}
