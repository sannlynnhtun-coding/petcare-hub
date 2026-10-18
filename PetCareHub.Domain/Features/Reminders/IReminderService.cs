using PetCareHub.Domain.Features.Reminders.Models;
using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Reminders;

public interface IReminderService
{
    // Commands
    Task<Result<ReminderDto>> CreateAsync(Guid userId, CreateReminderRequest request);
    Task<Result> CompleteAsync(Guid userId, Guid reminderId);

    // Queries
    Task<Result<List<ReminderDto>>> GetUpcomingAsync(Guid userId);
}
