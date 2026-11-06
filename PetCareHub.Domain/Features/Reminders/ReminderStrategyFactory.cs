using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Reminders;

// Factory + Strategy combination
public class ReminderStrategyFactory
{
    private readonly Dictionary<string, IReminderStrategy> _strategies;

    public ReminderStrategyFactory()
    {
        IReminderStrategy[] strategies =
        [
            new VaccinationReminderStrategy(),
            new MedicineReminderStrategy(),
            new GroomingReminderStrategy(),
            new CheckupReminderStrategy(),
            new DefaultReminderStrategy()
        ];
        _strategies = strategies.ToDictionary(strategy => strategy.Type, StringComparer.OrdinalIgnoreCase);
    }

    public IReminderStrategy GetStrategy(string type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
            return strategy;

        return _strategies[ReminderTypes.Custom];
    }
}
