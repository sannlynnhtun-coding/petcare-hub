using PetCareHub.Domain.Common;

namespace PetCareHub.Domain.Features.Reminders;

public class ReminderCompletedEvent : IDomainEvent
{
    public Guid ReminderId { get; }
    public string Title { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ReminderCompletedEvent(Guid reminderId, string title)
    {
        ReminderId = reminderId;
        Title = title;
    }
}
