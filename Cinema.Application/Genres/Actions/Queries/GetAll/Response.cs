using Cinema.Application.Genres.Dtos;
using Cinema.Domain.Genres.Entities;

namespace Cinema.Application.Genres.Actions.Queries.GetAll;

public class Response
{
    IEnumerable<GenreDto> Genres { get; set; }

    public Response(IEnumerable<Genre> genres)
    {
        Genres = genres.Select(g => new GenreDto(g));
    }
}