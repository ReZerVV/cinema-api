using MediatR;

namespace Cinema.Application.Movies.Actions.Commands.Update;

public record Request(
    string? Name = null,
    string? EnName = null,
    string? Description = null,
    string? ShortDescription = null,
    string? Type = null,
    int? Year = null,
    int? MovieLength = null,
    string? Country = null,
    float? Rating = null,
    int? Votes = null,
    string? PosterDownloadUrl = null,
    string? BackdropDownloadUrl = null,
    string? VideoDownloadUrl = null,
    IEnumerable<string>? Genres = null) : IRequest<Response>
{
    public string Id { get; set; } = null;
}
