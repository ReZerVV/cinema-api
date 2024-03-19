using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Actions.Movies.Commands.Create;

public record Request(
    string Name,
    string Description,
    int Year) : IRequest
{
    public string EnName { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int MovieLength { get; set; } = 0;
    public string Country { get; set; } = string.Empty;
    public float Rating { get; set; } = 0.0f;
    public int Votes { get; set; } = 0;
    public IEnumerable<string> Genres { get; set; } = new List<string>();

    public IFormFile? Poster { get; set; } = null;
    public IFormFile? Backdrop { get; set; } = null;
    public IFormFile? Video { get; set; } = null;

    public string? PosterUrl { get; set; } = null;
    public string? BackdropUrl { get; set; } = null;
    public string? VideoUrl { get; set; } = null;
}