using Cinema.Application.Dtos;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Actions.Movies.Queries.GetHistories;

public class Response
{
    public IEnumerable<LoadingDto> Downloads { get; set; }
    public int PageCount { get; set; }

    public Response(IEnumerable<Movie> movies, string baseUrl, int pageCount)
    {
        Downloads = movies.Select(m => new LoadingDto(m, baseUrl));
        PageCount = pageCount;
    }
}
