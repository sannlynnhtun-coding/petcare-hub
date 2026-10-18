namespace PetCareHub.Domain.Common;

// Simple Observer implementation for learning
public class InMemoryEventDispatcher : IEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent)
    {
        // In a real app you would notify handlers.
        // For MVP we just log to console so juniors can see it working.
        Console.WriteLine($"[EVENT] {domainEvent.GetType().Name} at {domainEvent.OccurredOn:u}");
        return Task.CompletedTask;
    }
}
