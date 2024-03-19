using Cinema.Application.Dtos;
using MediatR;

namespace Cinema.Application.Actions.Movies.Queries.GetById;

public record Request(string Id) : IRequest<MovieDto>;
