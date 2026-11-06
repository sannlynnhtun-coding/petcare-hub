using Microsoft.EntityFrameworkCore;
using PetCareHub.Database.Entities;

namespace PetCareHub.Database.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly AppDbContext _db;

    public ReminderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Reminder?> GetByIdAsync(Guid userId, Guid id)
    {
        return await OwnedByUser(userId).FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reminder>> GetUpcomingByUserAsync(Guid userId, int days = 14)
    {
        var until = DateTime.UtcNow.Date.AddDays(days);

        return await PendingByUser(userId)
            .Where(r => r.DueDate <= until)
            .Include(r => r.Pet)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
    }

    public Task<List<Reminder>> GetPendingByUserAsync(Guid userId) =>
        PendingByUser(userId).Include(r => r.Pet).OrderBy(r => r.DueDate).ToListAsync();

    public Task<Reminder?> GetPendingByIdAsync(Guid userId, Guid id) =>
        PendingByUser(userId).FirstOrDefaultAsync(r => r.Id == id);

    public Task<int> TryUpdatePendingAsync(Guid userId, Guid id, string title, string type, DateTime dueDate, string? notes) =>
        PendingByUser(userId).Where(r => r.Id == id)
            .ExecuteUpdateAsync(update => update
                .SetProperty(r => r.Title, title)
                .SetProperty(r => r.Type, type)
                .SetProperty(r => r.DueDate, dueDate)
                .SetProperty(r => r.Notes, notes));

    private IQueryable<Reminder> PendingByUser(Guid userId) =>
        OwnedByUser(userId).Where(r => !r.IsCompleted);

    private IQueryable<Reminder> OwnedByUser(Guid userId) =>
        _db.Reminders.Where(r => r.Pet!.UserId == userId);

    public async Task AddAsync(Reminder reminder)
    {
        await _db.Reminders.AddAsync(reminder);
    }

    public Task<int> TryCompleteAsync(Guid userId, Guid id, DateTime completedAt)
    {
        return PendingByUser(userId).Where(r => r.Id == id)
            .ExecuteUpdateAsync(update => update
                .SetProperty(r => r.IsCompleted, true)
                .SetProperty(r => r.CompletedAt, completedAt));
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
