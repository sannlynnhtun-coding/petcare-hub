namespace PetCareHub.Domain.Common;

// Adapter Pattern - simple fake implementation for learning
// In real projects this would call Email/SMS provider
public class FakeNotificationAdapter : INotificationAdapter
{
    public Task<bool> SendAsync(string message)
    {
        Console.WriteLine($"[NOTIFICATION] {message}");
        return Task.FromResult(true);
    }
}
