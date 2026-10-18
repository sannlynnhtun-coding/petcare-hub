using PetCareHub.Database.Entities;

namespace PetCareHub.Domain.Features.Pets;

// Builder Pattern - step by step construction of Pet
public class PetBuilder
{
    private Guid _userId;
    private string _name = string.Empty;
    private string _species = string.Empty;
    private string? _breed;
    private DateTime? _dateOfBirth;
    private string? _gender;
    private string? _notes;
    private string? _photoUrl;

    public PetBuilder ForUser(Guid userId)
    {
        _userId = userId;
        return this;
    }

    public PetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PetBuilder WithSpecies(string species)
    {
        _species = species;
        return this;
    }

    public PetBuilder WithBreed(string? breed)
    {
        _breed = breed;
        return this;
    }

    public PetBuilder WithDateOfBirth(DateTime? dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public PetBuilder WithGender(string? gender)
    {
        _gender = gender;
        return this;
    }

    public PetBuilder WithNotes(string? notes)
    {
        _notes = notes;
        return this;
    }

    public PetBuilder WithPhotoUrl(string? photoUrl)
    {
        _photoUrl = photoUrl;
        return this;
    }

    public Pet Build()
    {
        if (string.IsNullOrWhiteSpace(_name))
            throw new InvalidOperationException("Pet name is required.");

        if (string.IsNullOrWhiteSpace(_species))
            throw new InvalidOperationException("Pet species is required.");

        return new Pet
        {
            Id = Guid.NewGuid(),
            UserId = _userId,
            Name = _name.Trim(),
            Species = _species.Trim(),
            Breed = _breed?.Trim(),
            DateOfBirth = _dateOfBirth,
            Gender = _gender?.Trim(),
            Notes = _notes?.Trim(),
            PhotoUrl = _photoUrl?.Trim(),
            CreatedAt = DateTime.UtcNow
        };
    }
}
