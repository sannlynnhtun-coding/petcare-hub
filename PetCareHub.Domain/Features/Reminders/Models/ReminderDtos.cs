namespace PetCareHub.Domain.Features.Reminders.Models;

public record CreateReminderRequest(
    Guid PetId,
    string Type,
    string Title,
    string? Notes,
    DateTime DueDate
);

public record ReminderEditFields(string Type, string Title, string? Notes, DateTime DueDate);
