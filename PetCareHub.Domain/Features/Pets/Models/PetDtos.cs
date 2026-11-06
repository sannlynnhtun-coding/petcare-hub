namespace PetCareHub.Domain.Features.Pets.Models;

public record CreatePetRequest(
    string Name,
    string Species,
    string? Breed,
    DateTime? DateOfBirth,
    string? Gender,
    string? Notes,
    string? PhotoUrl
);

public record UpdatePetRequest(
    string Name,
    string Species,
    string? Breed,
    DateTime? DateOfBirth,
    string? Gender,
    string? Notes,
    string? PhotoUrl
);
