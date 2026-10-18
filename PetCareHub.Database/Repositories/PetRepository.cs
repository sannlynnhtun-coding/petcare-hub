using Microsoft.EntityFrameworkCore;
using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

// Repository Pattern implementation
public class PetRepository : IPetRepository
{
    private readonly AppDbContext _db;

    public PetRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Pet?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _db.Pets
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
    }

    public async Task<List<Pet>> GetAllByUserAsync(Guid userId)
    {
        return await _db.Pets
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Pet pet)
    {
        await _db.Pets.AddAsync(pet);
    }

    public async Task UpdateAsync(Pet pet)
    {
        _db.Pets.Update(pet);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
