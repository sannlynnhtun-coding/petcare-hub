using PetCareHub.Shared;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace PetCareHub.WebApp.Models;

// Shared Result from API
public class ApiResult<T> : ApiResult
{
    public T? Data { get; set; }
}

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Pet
public class PetProfileRequest
{
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Notes { get; set; }
    public string? PhotoUrl { get; set; }
}

// Reminder
public class CreateReminderRequest : ReminderEditFields
{
    public Guid PetId { get; set; }

    [BindNever, JsonIgnore]
    public IReadOnlyList<PetDto> Pets { get; set; } = [];
}

public class ReminderEditFields
{
    public string Type { get; set; } = ReminderTypes.Custom;
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    [BindRequired]
    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(7);
}
