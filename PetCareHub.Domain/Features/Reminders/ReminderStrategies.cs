namespace PetCareHub.Domain.Features.Reminders;

// Strategy Pattern - concrete strategies
public class VaccinationReminderStrategy : IReminderStrategy
{
    public string Type => "Vaccination";

    public string GetDisplayTitle(string title) => $"[Vaccination] {title}";

    public bool IsOverdue(DateTime dueDate) => dueDate.Date < DateTime.UtcNow.Date;
}

public class MedicineReminderStrategy : IReminderStrategy
{
    public string Type => "Medicine";

    public string GetDisplayTitle(string title) => $"[Medicine] {title}";

    public bool IsOverdue(DateTime dueDate) => dueDate.Date < DateTime.UtcNow.Date;
}

public class GroomingReminderStrategy : IReminderStrategy
{
    public string Type => "Grooming";

    public string GetDisplayTitle(string title) => $"[Grooming] {title}";

    public bool IsOverdue(DateTime dueDate) => dueDate.Date < DateTime.UtcNow.Date;
}

public class CheckupReminderStrategy : IReminderStrategy
{
    public string Type => "Checkup";

    public string GetDisplayTitle(string title) => $"[Checkup] {title}";

    public bool IsOverdue(DateTime dueDate) => dueDate.Date < DateTime.UtcNow.Date;
}

public class DefaultReminderStrategy : IReminderStrategy
{
    public string Type => "Custom";

    public string GetDisplayTitle(string title) => title;

    public bool IsOverdue(DateTime dueDate) => dueDate.Date < DateTime.UtcNow.Date;
}
