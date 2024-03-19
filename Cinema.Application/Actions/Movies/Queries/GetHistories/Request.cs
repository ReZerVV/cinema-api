using MediatR;

namespace Cinema.Application.Actions.Movies.Queries.GetHistories;

public record Request(
    int Limit,
    int Page) : IRequest<Response>;
