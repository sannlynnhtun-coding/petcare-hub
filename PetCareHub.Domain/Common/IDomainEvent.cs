namespace PetCareHub.Domain.Common;

// Observer Pattern - Domain Event
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
