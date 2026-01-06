namespace Shared.Events;

public record UserRegisteredEvent(Guid UserId, string Email, string Username, string ConfirmationLink);
