using PetCareHub.Shared;
using System.Net;
using System.Net.Http.Json;
using PetCareHub.WebApp.Models;

namespace PetCareHub.WebApp.Services;

public class PetCareApiClient
{
    private readonly HttpClient _http;

    public PetCareApiClient(HttpClient http)
    {
        _http = http;
    }

    // ===== Pets =====
    public async Task<ApiResult<List<PetDto>>?> GetPetsAsync()
    {
        return await _http.GetFromJsonAsync<ApiResult<List<PetDto>>>("api/pets");
    }

    public Task<ApiResult<PetDto>?> GetPetAsync(Guid id) =>
        GetOptionalAsync<PetDto>($"api/pets/{id}");

    public async Task<ApiResult<PetDto>?> CreatePetAsync(PetProfileRequest request)
    {
        using var response = await _http.PostAsJsonAsync("api/pets", request);
        return await response.Content.ReadFromJsonAsync<ApiResult<PetDto>>();
    }

    public async Task<ApiResult?> UpdatePetAsync(Guid id, PetProfileRequest request)
    {
        using var response = await _http.PutAsJsonAsync($"api/pets/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }

    // ===== Reminders =====
    public async Task<ApiResult<List<ReminderDto>>?> GetUpcomingRemindersAsync()
    {
        return await _http.GetFromJsonAsync<ApiResult<List<ReminderDto>>>("api/reminders/upcoming");
    }

    public Task<ApiResult<List<ReminderDto>>?> GetPendingRemindersAsync() =>
        _http.GetFromJsonAsync<ApiResult<List<ReminderDto>>>("api/reminders/pending");

    public Task<ApiResult<ReminderEditFields>?> GetReminderForEditAsync(Guid id) =>
        GetOptionalAsync<ReminderEditFields>($"api/reminders/{id}");

    public async Task<ApiResult?> UpdateReminderAsync(Guid id, ReminderEditFields request)
    {
        using var response = await _http.PutAsJsonAsync($"api/reminders/{id}", request);
        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }

    public async Task<ApiResult<ReminderDto>?> CreateReminderAsync(CreateReminderRequest request)
    {
        using var response = await _http.PostAsJsonAsync("api/reminders", request);
        return await response.Content.ReadFromJsonAsync<ApiResult<ReminderDto>>();
    }

    public async Task<ApiResult?> CompleteReminderAsync(Guid id)
    {
        using var response = await _http.PostAsync($"api/reminders/{id}/complete", null);
        return await response.Content.ReadFromJsonAsync<ApiResult>();
    }

    private async Task<ApiResult<T>?> GetOptionalAsync<T>(string path)
    {
        using var response = await _http.GetAsync(path);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ApiResult<T>>();
    }
}
