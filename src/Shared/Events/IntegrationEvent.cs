namespace Shared.Event;

public class IntegrationEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}
