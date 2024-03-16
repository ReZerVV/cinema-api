using Cinema.Application.Utils.Api.Kinopoisk.Models;
using Cinema.Application.Utils.Api.Kinopoisk.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Cinema.Application.Utils.Api.Kinopoisk;

public class KinopoiskApi
{
    private readonly HttpClient _client;

    public KinopoiskApi(IOptions<KinopoiskApiOptions> options)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://api.kinopoisk.dev/v1.4/");
        _client.DefaultRequestHeaders.Add("X-API-KEY", options.Value.ApiKey);
        _client.DefaultRequestHeaders.Add("accept", "application/json");
    }

    public async Task<FilmInfo> GetFilmById(string id)
    {
        var response = _client.Send(new HttpRequestMessage(HttpMethod.Get, $"movie/{id}"));
        if (!response.IsSuccessStatusCode)
            throw new CinemaError(
                Errors.CinemaErrorType.SYSTEM_ERROR,
                "The system failed to retrieve information about the movie from the link. Please try again later or upload the movie manually.");
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<FilmInfo>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive=true,
        });
        return json;
    }
}
