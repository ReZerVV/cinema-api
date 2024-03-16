using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cinema.Application.Actions.Movies.Commands.Create;

public record Request(
    string Name,
    string EnName,
    string Description,
    string ShortDescription,
    string Type,
    int Year,
    int MovieLength,
    string Country,
    float Rating,
    int Votes,
    IFormFile Poster,
    IFormFile Backdrop,
    IFormFile Video,
    IEnumerable<string> Genres) : IRequest;