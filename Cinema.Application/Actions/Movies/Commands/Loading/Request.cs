using MediatR;

namespace Cinema.Application.Actions.Movies.Commands.Loading;

public record Request(string Url) : IRequest;