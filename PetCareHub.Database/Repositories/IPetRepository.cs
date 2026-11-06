using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

// Repository Pattern
public interface IPetRepository
{
    // Returns a tracked pet for updates persisted by SaveChangesAsync.
    Task<Pet?> GetByIdAsync(Guid id, Guid userId);
    Task<List<Pet>> GetAllByUserAsync(Guid userId);
    Task AddAsync(Pet pet);
    Task SaveChangesAsync();
}
