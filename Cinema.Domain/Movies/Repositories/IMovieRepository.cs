﻿using Cinema.Domain.Common;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Domain.Movies.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    IEnumerable<Movie> Search(string? query = null, string? sort = null, string? type = null, string? genre = null);
}
