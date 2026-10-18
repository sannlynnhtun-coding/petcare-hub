namespace PetCareHub.Domain.Common;

// Adapter Pattern - abstract external notification service
public interface INotificationAdapter
{
    Task<bool> SendAsync(string message);
}
