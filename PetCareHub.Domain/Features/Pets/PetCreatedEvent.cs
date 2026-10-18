using PetCareHub.Domain.Common;

namespace PetCareHub.Domain.Features.Pets;

// Observer Pattern - concrete domain event
public class PetCreatedEvent : IDomainEvent
{
    public Guid PetId { get; }
    public string PetName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public PetCreatedEvent(Guid petId, string petName)
    {
        PetId = petId;
        PetName = petName;
    }
}
