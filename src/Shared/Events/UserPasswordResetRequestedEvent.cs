namespace Shared.Events;

public record UserPasswordResetRequestedEvent(string Email, string ResetLink);
