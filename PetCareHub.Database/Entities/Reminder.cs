namespace PetCareHub.Database.Entities;

public class Reminder
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public string Type { get; set; } = string.Empty; // Vaccination, Medicine, Grooming, Checkup, Custom
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Pet? Pet { get; set; }
}
