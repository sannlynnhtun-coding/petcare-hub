using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

public interface IReminderRepository
{
    Task<Reminder?> GetByIdAsync(Guid id);
    Task<List<Reminder>> GetUpcomingByUserAsync(Guid userId, int days = 14);
    Task<List<Reminder>> GetByPetAsync(Guid petId);
    Task AddAsync(Reminder reminder);
    Task UpdateAsync(Reminder reminder);
    Task SaveChangesAsync();
}
