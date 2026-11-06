using PetCareHub.Shared;

namespace PetCareHub.WebApp.Models;

public record DashboardModel(IReadOnlyList<PetDto> Pets, IReadOnlyList<ReminderDto> Reminders);
