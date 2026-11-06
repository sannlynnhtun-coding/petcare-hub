namespace PetCareHub.Shared;

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
