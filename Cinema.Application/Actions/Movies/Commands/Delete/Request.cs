using MediatR;

namespace Cinema.Application.Actions.Movies.Commands.Delete;

public record Request(string Id) : IRequest;
