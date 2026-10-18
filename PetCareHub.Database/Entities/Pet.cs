namespace PetCareHub.Database.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty; // Dog, Cat, etc.
    public string? Breed { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Notes { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public List<HealthLog> HealthLogs { get; set; } = new();
    public List<Reminder> Reminders { get; set; } = new();
}
