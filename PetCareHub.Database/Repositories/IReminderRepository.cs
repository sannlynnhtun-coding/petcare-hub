using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

public interface IReminderRepository
{
    Task<Reminder?> GetByIdAsync(Guid userId, Guid id);
    Task<List<Reminder>> GetUpcomingByUserAsync(Guid userId, int days = 14);
    Task<List<Reminder>> GetPendingByUserAsync(Guid userId);
    Task<Reminder?> GetPendingByIdAsync(Guid userId, Guid id);
    Task<int> TryUpdatePendingAsync(Guid userId, Guid id, string title, string type, DateTime dueDate, string? notes);
    Task AddAsync(Reminder reminder);
    Task<int> TryCompleteAsync(Guid userId, Guid id, DateTime completedAt);
    Task SaveChangesAsync();
}
