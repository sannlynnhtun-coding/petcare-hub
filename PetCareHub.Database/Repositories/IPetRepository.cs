using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

// Repository Pattern
public interface IPetRepository
{
    Task<Pet?> GetByIdAsync(Guid id, Guid userId);
    Task<List<Pet>> GetAllByUserAsync(Guid userId);
    Task AddAsync(Pet pet);
    Task UpdateAsync(Pet pet);
    Task SaveChangesAsync();
}
