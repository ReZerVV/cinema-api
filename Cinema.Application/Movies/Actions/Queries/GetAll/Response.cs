using Cinema.Application.Movies.Dtos;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Movies.Actions.Queries.GetAll;

public class Response
{
    public IEnumerable<MovieDto> Movies { get; set; }

    public Response(IEnumerable<Movie> movies, string baseUrl)
    {
        Movies = movies.Select(m => new MovieDto(m, baseUrl));
    }
}
