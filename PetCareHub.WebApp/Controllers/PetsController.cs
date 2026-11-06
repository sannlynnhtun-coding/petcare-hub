using PetCareHub.Shared;
using Microsoft.AspNetCore.Mvc;
using PetCareHub.WebApp.Models;
using PetCareHub.WebApp.Services;

namespace PetCareHub.WebApp.Controllers;

public class PetsController : Controller
{
    private readonly PetCareApiClient _api;

    public PetsController(PetCareApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _api.GetPetsAsync();
        return View(result?.Data ?? new List<PetDto>());
    }

    public IActionResult Create()
    {
        return View(new PetProfileRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(PetProfileRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _api.CreatePetAsync(model);

        if (result is null || !result.IsSuccess)
        {
            ViewBag.Error = result?.Message ?? "Failed to create pet.";
            return View(model);
        }

        TempData["Success"] = "Pet created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        var result = await _api.GetPetAsync(id);
        if (result?.Data is not { } pet || !result.IsSuccess)
            return NotFound();

        return View(new PetProfileRequest
        {
            Name = pet.Name,
            Species = pet.Species,
            Breed = pet.Breed,
            DateOfBirth = pet.DateOfBirth,
            Gender = pet.Gender,
            Notes = pet.Notes,
            PhotoUrl = pet.PhotoUrl
        });
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, PetProfileRequest model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _api.UpdatePetAsync(id, model);
        if (result?.IsSuccess != true)
        {
            ModelState.AddModelError(string.Empty, string.IsNullOrEmpty(result?.Message)
                ? "Failed to update pet."
                : result.Message);
            return View(model);
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _api.GetPetAsync(id);
        if (result is null || !result.IsSuccess || result.Data is null)
            return RedirectToAction(nameof(Index));

        return View(result.Data);
    }
}
