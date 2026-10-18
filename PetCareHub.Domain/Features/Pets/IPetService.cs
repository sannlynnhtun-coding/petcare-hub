using PetCareHub.Domain.Features.Pets.Models;
using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Pets;

// CQRS style: Commands and Queries in the same service for simplicity (junior friendly)
public interface IPetService
{
    // Commands (Write)
    Task<Result<PetDto>> CreateAsync(Guid userId, CreatePetRequest request);
    Task<Result> UpdateAsync(Guid userId, Guid petId, UpdatePetRequest request);

    // Queries (Read)
    Task<Result<PetDto>> GetByIdAsync(Guid userId, Guid petId);
    Task<Result<List<PetDto>>> GetAllAsync(Guid userId);
}
