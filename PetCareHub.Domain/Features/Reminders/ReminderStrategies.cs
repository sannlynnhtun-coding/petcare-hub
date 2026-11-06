using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Reminders;

// Strategy Pattern - concrete strategies
public class VaccinationReminderStrategy : IReminderStrategy
{
    public string Type => ReminderTypes.Vaccination;

    public string GetDisplayTitle(string title) => $"[{Type}] {title}";
}

public class MedicineReminderStrategy : IReminderStrategy
{
    public string Type => ReminderTypes.Medicine;

    public string GetDisplayTitle(string title) => $"[{Type}] {title}";
}

public class GroomingReminderStrategy : IReminderStrategy
{
    public string Type => ReminderTypes.Grooming;

    public string GetDisplayTitle(string title) => $"[{Type}] {title}";
}

public class CheckupReminderStrategy : IReminderStrategy
{
    public string Type => ReminderTypes.Checkup;

    public string GetDisplayTitle(string title) => $"[{Type}] {title}";
}

public class DefaultReminderStrategy : IReminderStrategy
{
    public string Type => ReminderTypes.Custom;

    public string GetDisplayTitle(string title) => title;
}
