using PetCareHub.Shared;
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
        var result = await _api.GetPendingRemindersAsync();
        return View(result?.Data ?? new List<ReminderDto>());
    }

    public async Task<IActionResult> Create()
    {
        return await CreateViewAsync(new CreateReminderRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReminderRequest model)
    {
        if (!ModelState.IsValid)
            return await CreateViewAsync(model);

        var result = await _api.CreateReminderAsync(model);

        if (result is null || !result.IsSuccess)
        {
            ViewBag.Error = result?.Message ?? "Failed to create reminder.";
            return await CreateViewAsync(model);
        }

        TempData["Success"] = "Reminder created successfully!";
        return RedirectToAction(nameof(Index));
    }

    private async Task<IActionResult> CreateViewAsync(CreateReminderRequest model)
    {
        var pets = await _api.GetPetsAsync();
        model.Pets = pets?.Data ?? [];
        return View(nameof(Create), model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        var result = await _api.GetReminderForEditAsync(id);
        if (result?.Data is null)
        {
            TempData["Error"] = "Pending reminder not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, ReminderEditFields model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _api.UpdateReminderAsync(id, model);
        if (result?.IsSuccess != true)
        {
            ModelState.AddModelError(string.Empty, string.IsNullOrEmpty(result?.Message)
                ? "Failed to update reminder."
                : result.Message);
            return View(model);
        }

        TempData["Success"] = result.Message;
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
