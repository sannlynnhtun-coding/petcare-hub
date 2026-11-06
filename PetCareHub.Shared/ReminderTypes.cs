namespace PetCareHub.Shared;

public static class ReminderTypes
{
    public const string Vaccination = "Vaccination";
    public const string Medicine = "Medicine";
    public const string Grooming = "Grooming";
    public const string Checkup = "Checkup";
    public const string Custom = "Custom";

    public static IReadOnlyList<string> All { get; } =
        Array.AsReadOnly(new[] { Vaccination, Medicine, Grooming, Checkup, Custom });
}
