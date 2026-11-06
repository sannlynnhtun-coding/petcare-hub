using PetCareHub.Database.Entities;
using PetCareHub.Shared;

namespace PetCareHub.Database;

public static class DbInitializer
{
    // For MVP we use a fixed demo user. In a real app use JWT claims.
    public static readonly Guid DemoUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static void Seed(AppDbContext db)
    {
        if (db.Pets.Any())
            return;

        var user = new User
        {
            Id = DemoUserId,
            Email = "demo@petcarehub.com",
            PasswordHash = "demo",
            DisplayName = "Demo Owner",
            CreatedAt = DateTime.UtcNow.AddDays(-90)
        };
        db.Users.Add(user);

        // ========== 15 PETS ==========
        var pets = new List<Pet>
        {
            MakePet("Max", "Dog", "Golden Retriever", "Male", new DateTime(2019, 3, 14), "Loves swimming and fetch."),
            MakePet("Luna", "Cat", "Siamese", "Female", new DateTime(2020, 7, 22), "Quiet, sits by the window."),
            MakePet("Buddy", "Dog", "Beagle", "Male", new DateTime(2018, 11, 5), "Very energetic, needs daily walks."),
            MakePet("Milo", "Cat", "Maine Coon", "Male", new DateTime(2021, 1, 18), "Large and friendly with kids."),
            MakePet("Bella", "Dog", "Labrador Retriever", "Female", new DateTime(2020, 4, 9), "Gentle, good with other dogs."),
            MakePet("Oliver", "Cat", "British Shorthair", "Male", new DateTime(2019, 9, 30), "Calm and independent."),
            MakePet("Charlie", "Dog", "French Bulldog", "Male", new DateTime(2021, 6, 12), "Playful but tires easily."),
            MakePet("Lucy", "Cat", "Ragdoll", "Female", new DateTime(2022, 2, 3), "Loves being held."),
            MakePet("Cooper", "Dog", "Border Collie", "Male", new DateTime(2018, 8, 21), "High energy, needs mental stimulation."),
            MakePet("Daisy", "Dog", "Cocker Spaniel", "Female", new DateTime(2020, 12, 1), "Friendly and affectionate."),
            MakePet("Simba", "Cat", "Bengal", "Male", new DateTime(2021, 5, 15), "Very active, likes climbing."),
            MakePet("Rocky", "Dog", "German Shepherd", "Male", new DateTime(2017, 10, 8), "Well trained, protective."),
            MakePet("Coco", "Dog", "Poodle", "Female", new DateTime(2022, 3, 27), "Hypoallergenic coat."),
            MakePet("Nala", "Cat", "Persian", "Female", new DateTime(2019, 4, 11), "Needs regular grooming."),
            MakePet("Teddy", "Dog", "Shih Tzu", "Male", new DateTime(2020, 9, 19), "Small, good apartment dog.")
        };
        db.Pets.AddRange(pets);

        // ========== 20 HEALTH LOGS ==========
        var healthLogs = new List<HealthLog>
        {
            MakeLog(pets[0], "Vaccination", "Rabies vaccine", "Annual dose", null, -60),
            MakeLog(pets[0], "Weight", "Weight check", "Healthy range", 32.4m, -14),
            MakeLog(pets[0], "VetVisit", "Annual physical exam", "All clear", null, -45),
            MakeLog(pets[1], "Vaccination", "FVRCP booster", "Core vaccine", null, -90),
            MakeLog(pets[1], "Weight", "Weight check", "Slightly underweight", 3.8m, -20),
            MakeLog(pets[2], "Medication", "Flea and tick treatment", "Monthly topical", null, -7),
            MakeLog(pets[2], "VetVisit", "Ear infection follow-up", "Cleared up", null, -30),
            MakeLog(pets[3], "Vaccination", "Rabies vaccine", "3-year vaccine", null, -120),
            MakeLog(pets[3], "Weight", "Weight check", "Normal", 6.2m, -10),
            MakeLog(pets[4], "Vaccination", "DHPP vaccine", "Booster", null, -50),
            MakeLog(pets[4], "VetVisit", "Spay surgery checkup", "Healing well", null, -180),
            MakeLog(pets[5], "Medication", "Deworming", "Routine", null, -25),
            MakeLog(pets[6], "VetVisit", "Breathing assessment", "Mild brachycephalic issues noted", null, -40),
            MakeLog(pets[6], "Weight", "Weight check", "Ideal", 11.5m, -8),
            MakeLog(pets[7], "Vaccination", "FVRCP", "Kitten series complete", null, -200),
            MakeLog(pets[8], "VetVisit", "Hip evaluation", "No dysplasia signs", null, -70),
            MakeLog(pets[8], "Medication", "Joint supplement started", "Glucosamine daily", null, -15),
            MakeLog(pets[9], "Vaccination", "Bordetella", "Kennel cough", null, -35),
            MakeLog(pets[11], "VetVisit", "Senior wellness panel", "Bloodwork normal", null, -55),
            MakeLog(pets[11], "Weight", "Weight check", "Slightly overweight", 38.0m, -12)
        };
        db.HealthLogs.AddRange(healthLogs);

        // ========== 15 REMINDERS ==========
        var reminders = new List<Reminder>
        {
            MakeRem(pets[0], ReminderTypes.Vaccination, "Annual rabies booster", "Book vet appointment", 5),
            MakeRem(pets[1], ReminderTypes.Grooming, "Nail trimming", "Every 4 to 6 weeks", 3),
            MakeRem(pets[2], ReminderTypes.Medicine, "Heartworm prevention", "Monthly tablet", 1),
            MakeRem(pets[3], ReminderTypes.Checkup, "Dental cleaning", "Recommended by vet", 12),
            MakeRem(pets[4], ReminderTypes.Vaccination, "DHPP booster due", null, 18),
            MakeRem(pets[5], ReminderTypes.Medicine, "Monthly deworming", null, 6),
            MakeRem(pets[6], ReminderTypes.Checkup, "Breathing follow-up", "Monitor during hot weather", 9),
            MakeRem(pets[7], ReminderTypes.Grooming, "Full grooming session", "Long coat care", 4),
            MakeRem(pets[8], ReminderTypes.Medicine, "Joint supplement refill", "Running low", 2),
            MakeRem(pets[9], ReminderTypes.Vaccination, "Bordetella annual", null, 25),
            MakeRem(pets[10], ReminderTypes.Grooming, "Nail and ear cleaning", null, 7),
            MakeRem(pets[11], ReminderTypes.Checkup, "Senior bloodwork", "Yearly panel", 14),
            MakeRem(pets[12], ReminderTypes.Grooming, "Haircut appointment", "Poodle coat", 8),
            MakeRem(pets[13], ReminderTypes.Grooming, "Persian coat brushing", "Daily brush reminder", 1),
            MakeRem(pets[14], ReminderTypes.Vaccination, "Rabies vaccine due", null, 22)
        };
        db.Reminders.AddRange(reminders);

        db.SaveChanges();
    }

    private static Pet MakePet(string name, string species, string breed, string gender, DateTime dob, string notes) =>
        new()
        {
            Id = Guid.NewGuid(),
            UserId = DemoUserId,
            Name = name,
            Species = species,
            Breed = breed,
            Gender = gender,
            DateOfBirth = dob,
            Notes = notes,
            CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(10, 80))
        };

    private static HealthLog MakeLog(Pet pet, string type, string title, string? notes, decimal? value, int daysAgo) =>
        new()
        {
            Id = Guid.NewGuid(),
            PetId = pet.Id,
            Type = type,
            Title = title,
            Notes = notes,
            Value = value,
            Date = DateTime.UtcNow.Date.AddDays(daysAgo),
            CreatedAt = DateTime.UtcNow.AddDays(daysAgo)
        };

    private static Reminder MakeRem(Pet pet, string type, string title, string? notes, int dueInDays) =>
        new()
        {
            Id = Guid.NewGuid(),
            PetId = pet.Id,
            Type = type,
            Title = title,
            Notes = notes,
            DueDate = DateTime.UtcNow.Date.AddDays(dueInDays),
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 14))
        };
}
