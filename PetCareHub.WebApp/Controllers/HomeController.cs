using Microsoft.AspNetCore.Mvc;
using PetCareHub.WebApp.Services;
using PetCareHub.WebApp.Models;

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

        return View(new DashboardModel(petsResult?.Data ?? [], remindersResult?.Data ?? []));
    }
}
