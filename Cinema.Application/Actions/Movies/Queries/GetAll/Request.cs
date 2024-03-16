using MediatR;

namespace Cinema.Application.Actions.Movies.Queries.GetAll;

public record Request() : IRequest<Response>;
