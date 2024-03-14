using MediatR;

namespace Cinema.Application.Movies.Actions.Queries.GetDownloads;

public record Request() : IRequest<Response>;
