namespace PetCareHub.Domain.Features.Reminders.Models;

public record CreateReminderRequest(
    Guid PetId,
    string Type,
    string Title,
    string? Notes,
    DateTime DueDate
);

public record ReminderDto(
    Guid Id,
    Guid PetId,
    string PetName,
    string Type,
    string Title,
    string? Notes,
    DateTime DueDate,
    bool IsCompleted,
    DateTime? CompletedAt
);
