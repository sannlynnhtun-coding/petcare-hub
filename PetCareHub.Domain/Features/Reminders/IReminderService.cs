using PetCareHub.Domain.Features.Reminders.Models;
using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Reminders;

public interface IReminderService
{
    // Commands
    Task<Result<ReminderDto>> CreateAsync(Guid userId, CreateReminderRequest request);
    Task<Result> CompleteAsync(Guid userId, Guid reminderId);

    Task<Result> UpdatePendingAsync(Guid userId, Guid id, ReminderEditFields request);

    // Queries
    Task<Result<List<ReminderDto>>> GetPendingAsync(Guid userId);
    Task<Result<ReminderEditFields>> GetPendingForEditAsync(Guid userId, Guid id);
    Task<Result<List<ReminderDto>>> GetUpcomingAsync(Guid userId);
}
