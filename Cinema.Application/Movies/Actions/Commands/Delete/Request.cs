using MediatR;

namespace Cinema.Application.Movies.Actions.Commands.Delete;

public record Request(string Id) : IRequest<Response>;
