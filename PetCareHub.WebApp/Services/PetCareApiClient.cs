using System.Net;
using System.Net.Http.Json;
using PetCareHub.WebApp.Models;

namespace PetCareHub.WebApp.Services;

public class PetCareApiClient
{
    private readonly HttpClient _http;

    public PetCareApiClient(IHttpClientFactory httpClientFactory)
    {
        _http = httpClientFactory.CreateClient("PetCareApi");
    }

    // ===== Pets =====
    public async Task<ApiResult<List<PetDto>>?> GetPetsAsync()
    {
        return await _http.GetFromJsonAsync<ApiResult<List<PetDto>>>("api/pets");
    }

    public async Task<ApiResult<PetDto>?> GetPetAsync(Guid id)
    {
        using var response = await _http.GetAsync($"api/pets/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApiResult<PetDto>>();
    }

    public async Task<ApiResult<PetDto>?> CreatePetAsync(CreatePetRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/pets", request);
        return await response.Content.ReadFromJsonAsync<ApiResult<PetDto>>();
    }

    // ===== Reminders =====
    public async Task<ApiResult<List<ReminderDto>>?> GetUpcomingRemindersAsync()
    {
        return await _http.GetFromJsonAsync<ApiResult<List<ReminderDto>>>("api/reminders/upcoming");
    }

    public Task<ApiResult<List<ReminderDto>>?> GetPendingRemindersAsync() =>
        _http.GetFromJsonAsync<ApiResult<List<ReminderDto>>>("api/reminders/pending");

    public async Task<ApiResult<ReminderDto>?> CreateReminderAsync(CreateReminderRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/reminders", request);
        return await response.Content.ReadFromJsonAsync<ApiResult<ReminderDto>>();
    }

    public async Task<ApiResult?> CompleteReminderAsync(Guid id)
    {
        var response = await _http.PostAsync($"api/reminders/{id}/complete", null);
        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }
}
