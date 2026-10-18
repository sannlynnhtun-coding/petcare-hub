using PetCareHub.Database.Entities;
using PetCareHub.Domain.Features.Pets.Models;

namespace PetCareHub.Domain.Features.Pets;

// Factory Pattern
public class PetFactory
{
    public Pet Create(Guid userId, CreatePetRequest request)
    {
        // Uses Builder internally for clean construction
        return new PetBuilder()
            .ForUser(userId)
            .WithName(request.Name)
            .WithSpecies(request.Species)
            .WithBreed(request.Breed)
            .WithDateOfBirth(request.DateOfBirth)
            .WithGender(request.Gender)
            .WithNotes(request.Notes)
            .WithPhotoUrl(request.PhotoUrl)
            .Build();
    }
}
