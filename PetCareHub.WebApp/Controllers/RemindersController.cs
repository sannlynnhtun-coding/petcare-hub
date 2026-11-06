using Microsoft.AspNetCore.Mvc;
using PetCareHub.WebApp.Models;
using PetCareHub.WebApp.Services;

namespace PetCareHub.WebApp.Controllers;

public class RemindersController : Controller
{
    private readonly PetCareApiClient _api;

    public RemindersController(PetCareApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _api.GetUpcomingRemindersAsync();
        return View(result?.Data ?? new List<ReminderDto>());
    }

    public async Task<IActionResult> Create()
    {
        // Load pets for dropdown
        var petsResult = await _api.GetPetsAsync();
        ViewBag.Pets = petsResult?.Data ?? new List<PetDto>();
        return View(new CreateReminderRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReminderRequest model)
    {
        var petsResult = await _api.GetPetsAsync();
        ViewBag.Pets = petsResult?.Data ?? new List<PetDto>();

        if (!ModelState.IsValid)
            return View(model);

        var result = await _api.CreateReminderAsync(model);

        if (result is null || !result.IsSuccess)
        {
            ViewBag.Error = result?.Message ?? "Failed to create reminder.";
            return View(model);
        }

        TempData["Success"] = "Reminder created successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Complete(Guid id)
    {
        var result = await _api.CompleteReminderAsync(id);
        if (result?.IsSuccess == true)
            TempData["Success"] = result.Message;
        else
            TempData["Error"] = result?.Message ?? "Failed to complete reminder.";
        return RedirectToAction(nameof(Index));
    }
}
