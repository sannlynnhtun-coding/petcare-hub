namespace PetCareHub.Domain.Features.Reminders;

// Strategy Pattern
public interface IReminderStrategy
{
    string Type { get; }
    string GetDisplayTitle(string title);
}
