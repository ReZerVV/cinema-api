using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Actions.Movies.Commands.Create;

public record Request(
    string Name,
    string EnName,
    string Description,
    string ShortDescription,
    string Type,
    int Year,
    int MovieLength,
    string Country,
    float Rating,
    int Votes,
    IEnumerable<string> Genres) : IRequest
{
    public IFormFile? Poster { get; set; } = null;
    public IFormFile? Backdrop { get; set; } = null;
    public IFormFile? Video { get; set; } = null;

    public string? PosterUrl { get; set; } = null;
    public string? BackdropUrl { get; set; } = null;
    public string? VideoUrl { get; set; } = null;
}