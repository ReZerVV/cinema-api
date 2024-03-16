using MediatR;

namespace Cinema.Application.Actions.Movies.Queries.GetLoadings;

public record Request() : IRequest<Response>;
