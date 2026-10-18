namespace PetCareHub.Database.Entities;

public class HealthLog
{
    public Guid Id { get; set; }
    public Guid PetId { get; set; }
    public string Type { get; set; } = string.Empty; // Vaccination, Weight, VetVisit, Medication, Other
    public string Title { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public decimal? Value { get; set; } // e.g. weight
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Pet? Pet { get; set; }
}
