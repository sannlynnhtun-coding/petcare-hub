namespace PetCareHub.Domain.Features.Reminders;

// Factory + Strategy combination
public class ReminderStrategyFactory
{
    private readonly Dictionary<string, IReminderStrategy> _strategies;

    public ReminderStrategyFactory()
    {
        _strategies = new Dictionary<string, IReminderStrategy>(StringComparer.OrdinalIgnoreCase)
        {
            ["Vaccination"] = new VaccinationReminderStrategy(),
            ["Medicine"] = new MedicineReminderStrategy(),
            ["Grooming"] = new GroomingReminderStrategy(),
            ["Custom"] = new DefaultReminderStrategy()
        };
    }

    public IReminderStrategy GetStrategy(string type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
            return strategy;

        return _strategies["Custom"];
    }
}
