using PetCareHub.Database.Entities;
using PetCareHub.Database.Repositories;
using PetCareHub.Domain.Common;
using PetCareHub.Domain.Features.Reminders.Models;
using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Reminders;

public class ReminderService : IReminderService
{
    private readonly IReminderRepository _reminderRepository;
    private readonly IPetRepository _petRepository;
    private readonly ReminderStrategyFactory _strategyFactory;
    private readonly IEventDispatcher _eventDispatcher;

    public ReminderService(
        IReminderRepository reminderRepository,
        IPetRepository petRepository,
        ReminderStrategyFactory strategyFactory,
        IEventDispatcher eventDispatcher)
    {
        _reminderRepository = reminderRepository;
        _petRepository = petRepository;
        _strategyFactory = strategyFactory;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Result<ReminderDto>> CreateAsync(Guid userId, CreateReminderRequest request)
    {
        var pet = await _petRepository.GetByIdAsync(request.PetId, userId);
        if (pet is null)
            return Result<ReminderDto>.Failure("Pet not found.");

        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<ReminderDto>.Failure("Title is required.");

        if (request.DueDate == default)
            return Result<ReminderDto>.Failure("Due date is required.");

        var strategy = _strategyFactory.GetStrategy(request.Type);

        var reminder = new Reminder
        {
            Id = Guid.NewGuid(),
            PetId = request.PetId,
            Type = strategy.Type,
            Title = request.Title.Trim(),
            Notes = request.Notes?.Trim(),
            DueDate = request.DueDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _reminderRepository.AddAsync(reminder);
        await _reminderRepository.SaveChangesAsync();

        var dto = new ReminderDto(
            reminder.Id,
            reminder.PetId,
            pet.Name,
            reminder.Type,
            strategy.GetDisplayTitle(reminder.Title),
            reminder.Notes,
            reminder.DueDate,
            reminder.IsCompleted,
            reminder.CompletedAt);

        return Result<ReminderDto>.Success(dto, "Reminder created.");
    }

    public async Task<Result> CompleteAsync(Guid userId, Guid reminderId)
    {
        var reminder = await _reminderRepository.GetByIdAsync(reminderId);
        if (reminder is null)
            return Result.Failure("Reminder not found.");

        // Simple ownership check via pet
        var pet = await _petRepository.GetByIdAsync(reminder.PetId, userId);
        if (pet is null)
            return Result.Failure("Reminder not found.");

        if (reminder.IsCompleted || await _reminderRepository.TryCompleteAsync(reminder.Id, DateTime.UtcNow) == 0)
            return Result.Failure("Reminder is already completed.");

        // Observer
        await _eventDispatcher.DispatchAsync(new ReminderCompletedEvent(reminder.Id, reminder.Title));

        return Result.Success("Reminder marked as completed.");
    }

    public async Task<Result<List<ReminderDto>>> GetUpcomingAsync(Guid userId)
    {
        var reminders = await _reminderRepository.GetUpcomingByUserAsync(userId);
        return Result<List<ReminderDto>>.Success(reminders.Select(MapForList).ToList());
    }

    public async Task<Result<List<ReminderDto>>> GetPendingAsync(Guid userId)
    {
        var reminders = await _reminderRepository.GetPendingByUserAsync(userId);
        return Result<List<ReminderDto>>.Success(reminders.Select(MapForList).ToList());
    }

    public async Task<Result<ReminderEditFields>> GetPendingForEditAsync(Guid userId, Guid id)
    {
        var reminder = await _reminderRepository.GetPendingByIdAsync(userId, id);
        return reminder is null
            ? Result<ReminderEditFields>.Failure("Pending reminder not found.")
            : Result<ReminderEditFields>.Success(new(reminder.Type, reminder.Title, reminder.Notes, reminder.DueDate));
    }

    public async Task<Result> UpdatePendingAsync(Guid userId, Guid id, ReminderEditFields request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result.Failure("Title is required.");

        if (request.DueDate == default)
            return Result.Failure("Due date is required.");

        if (string.IsNullOrWhiteSpace(request.Type))
            return Result.Failure("Type is required.");

        var type = _strategyFactory.GetStrategy(request.Type).Type;
        var updated = await _reminderRepository.TryUpdatePendingAsync(
            userId, id, request.Title.Trim(), type, request.DueDate, request.Notes?.Trim());
        return updated == 0
            ? Result.Failure("Pending reminder not found. It may have already been completed.")
            : Result.Success("Reminder updated.");
    }

    private ReminderDto MapForList(Reminder reminder) => new(
        reminder.Id,
        reminder.PetId,
        reminder.Pet?.Name ?? "Unknown",
        reminder.Type,
        _strategyFactory.GetStrategy(reminder.Type).GetDisplayTitle(reminder.Title),
        reminder.Notes,
        reminder.DueDate,
        reminder.IsCompleted,
        reminder.CompletedAt);
}
