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

    public async Task<Reminder?> GetByIdAsync(Guid id)
    {
        return await _db.Reminders.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<List<Reminder>> GetUpcomingByUserAsync(Guid userId, int days = 14)
    {
        var until = DateTime.UtcNow.Date.AddDays(days);

        return await _db.Reminders
            .Include(r => r.Pet)
            .Where(r => r.Pet!.UserId == userId && !r.IsCompleted && r.DueDate <= until)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
    }

    public async Task<List<Reminder>> GetByPetAsync(Guid petId)
    {
        return await _db.Reminders
            .Where(r => r.PetId == petId)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
    }

    public async Task AddAsync(Reminder reminder)
    {
        await _db.Reminders.AddAsync(reminder);
    }

    public async Task UpdateAsync(Reminder reminder)
    {
        _db.Reminders.Update(reminder);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
