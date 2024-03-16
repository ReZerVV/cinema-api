using Cinema.Domain.Genres.Entities;

namespace Cinema.Application.Dtos;

public class GenreDto
{
    public string Id { get; set; }
    public string Name { get; set; }

    public GenreDto(Genre genre)
    {
        Id = genre.Id;
        Name = genre.Name;
    }
}
