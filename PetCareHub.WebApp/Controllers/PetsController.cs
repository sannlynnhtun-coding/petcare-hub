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
        return View(new CreatePetRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePetRequest model)
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

    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _api.GetPetAsync(id);
        if (result is null || !result.IsSuccess || result.Data is null)
            return RedirectToAction(nameof(Index));

        return View(result.Data);
    }
}
