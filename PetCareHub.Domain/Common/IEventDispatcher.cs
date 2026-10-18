namespace PetCareHub.Domain.Common;

// Observer Pattern - simple event dispatcher
public interface IEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}
