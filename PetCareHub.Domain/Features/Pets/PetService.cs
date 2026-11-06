using PetCareHub.Database.Repositories;
using PetCareHub.Domain.Common;
using PetCareHub.Domain.Features.Pets.Models;
using PetCareHub.Shared;

namespace PetCareHub.Domain.Features.Pets;

public class PetService : IPetService
{
    private readonly IPetRepository _petRepository;
    private readonly PetFactory _petFactory;
    private readonly IEventDispatcher _eventDispatcher;

    public PetService(
        IPetRepository petRepository,
        PetFactory petFactory,
        IEventDispatcher eventDispatcher)
    {
        _petRepository = petRepository;
        _petFactory = petFactory;
        _eventDispatcher = eventDispatcher;
    }

    // Command
    public async Task<Result<PetDto>> CreateAsync(Guid userId, CreatePetRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<PetDto>.Failure("Pet name is required.");

        if (string.IsNullOrWhiteSpace(request.Species))
            return Result<PetDto>.Failure("Species is required.");

        var pet = _petFactory.Create(userId, request);

        await _petRepository.AddAsync(pet);
        await _petRepository.SaveChangesAsync();

        // Observer Pattern - raise event
        await _eventDispatcher.DispatchAsync(new PetCreatedEvent(pet.Id, pet.Name));

        return Result<PetDto>.Success(MapToDto(pet), "Pet created successfully.");
    }

    // Command
    public async Task<Result> UpdateAsync(Guid userId, Guid petId, UpdatePetRequest request)
    {
        var pet = await _petRepository.GetByIdAsync(petId, userId);
        if (pet is null)
            return Result.Failure("Pet not found.");

        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Species))
            return Result.Failure("Pet name and species are required.");

        pet.Name = request.Name.Trim();
        pet.Species = request.Species.Trim();
        pet.Breed = request.Breed?.Trim();
        pet.DateOfBirth = request.DateOfBirth;
        pet.Gender = request.Gender?.Trim();
        pet.Notes = request.Notes?.Trim();
        pet.PhotoUrl = request.PhotoUrl?.Trim();

        await _petRepository.UpdateAsync(pet);
        await _petRepository.SaveChangesAsync();

        return Result.Success("Pet updated successfully.");
    }

    // Query
    public async Task<Result<PetDto>> GetByIdAsync(Guid userId, Guid petId)
    {
        var pet = await _petRepository.GetByIdAsync(petId, userId);
        if (pet is null)
            return Result<PetDto>.Failure("Pet not found.");

        return Result<PetDto>.Success(MapToDto(pet));
    }

    // Query
    public async Task<Result<List<PetDto>>> GetAllAsync(Guid userId)
    {
        var pets = await _petRepository.GetAllByUserAsync(userId);
        var dtos = pets.Select(MapToDto).ToList();
        return Result<List<PetDto>>.Success(dtos);
    }

    private static PetDto MapToDto(Database.Entities.Pet pet) =>
        new(pet.Id, pet.Name, pet.Species, pet.Breed, pet.DateOfBirth,
            pet.Gender, pet.Notes, pet.PhotoUrl, pet.CreatedAt);
}
