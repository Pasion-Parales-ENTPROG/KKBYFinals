
using System.Text.Json;
using KKBYFinals.Models;

namespace KKBYFinals.Services;

public class HolidayService
{
    private readonly HttpClient _httpClient;

    public HolidayService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Holiday>> GetHolidaysAsync(int year)
    {
        var url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/PH";
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<Holiday>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        ) ?? new List<Holiday>();
    }
}
