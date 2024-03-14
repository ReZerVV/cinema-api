using Cinema.Application.Movies.Dtos;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Movies.Actions.Queries.GetDownloads;

public class Response
{
    public IEnumerable<DownloadDto> Downloads { get; set; }

    public Response(IEnumerable<Movie> movies)
    {
        Downloads = movies.Select(m => new DownloadDto(m));
    }
}
