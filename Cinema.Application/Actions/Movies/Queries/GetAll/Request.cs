using MediatR;

namespace Cinema.Application.Actions.Movies.Queries.GetAll;

public record Request(
    int Limit,
    int Page) : IRequest<Response>
{
    public string? Query { get; set; } = null;
    public string? Genre { get; set; } = null;
    public string? Type { get; set; } = null;
    public string? Sort { get; set; } = null;
}
