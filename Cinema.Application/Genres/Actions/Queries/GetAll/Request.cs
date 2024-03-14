using MediatR;

namespace Cinema.Application.Genres.Actions.Queries.GetAll;

public record Request() : IRequest<Response>;