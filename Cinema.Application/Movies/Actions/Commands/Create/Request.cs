﻿using MediatR;

namespace Cinema.Application.Movies.Actions.Commands.Create;

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
    string PosterDownloadUrl,
    string BackdropDownloadUrl,
    string VideoDownloadUrl,
    IEnumerable<string> Genres) : IRequest<Response>;