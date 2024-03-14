using MediatR;

namespace Cinema.Application.Movies.Actions.Queries.GetAll;

public record Request() : IRequest<Response>;
