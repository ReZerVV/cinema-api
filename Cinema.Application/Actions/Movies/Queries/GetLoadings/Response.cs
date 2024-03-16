using Cinema.Application.Dtos;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Actions.Movies.Queries.GetLoadings;

public class Response
{
    public IEnumerable<LoadingDto> Downloads { get; set; }

    public Response(IEnumerable<Movie> movies, string baseUrl)
    {
        Downloads = movies.Select(m => new LoadingDto(m, baseUrl));
    }
}
