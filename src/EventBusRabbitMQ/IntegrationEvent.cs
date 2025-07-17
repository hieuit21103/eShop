namespace EventBusRabbitMQ;

public abstract class IntegrationEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now.AddHours(7);
}
