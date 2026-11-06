namespace PetCareHub.Shared;

public record PetDto(
    Guid Id,
    string Name,
    string Species,
    string? Breed,
    DateTime? DateOfBirth,
    string? Gender,
    string? Notes,
    string? PhotoUrl,
    DateTime CreatedAt
);
