using Microsoft.AspNetCore.Mvc;
using PetCareHub.WebApp.Services;

namespace PetCareHub.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly PetCareApiClient _api;

    public HomeController(PetCareApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var petsResult = await _api.GetPetsAsync();
        var remindersResult = await _api.GetUpcomingRemindersAsync();

        ViewBag.Pets = petsResult?.Data ?? new();
        ViewBag.Reminders = remindersResult?.Data ?? new();
        ViewBag.PetCount = petsResult?.Data?.Count ?? 0;
        ViewBag.ReminderCount = remindersResult?.Data?.Count ?? 0;

        return View();
    }
}
