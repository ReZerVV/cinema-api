using Cinema.Domain.Common;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Domain.Genres.Entities;

public class Genre : AggregateRoot
{
    public string Name { get; set; }
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public Genre() : base(Guid.NewGuid().ToString())
    {

    }

    public static Genre Create(string name)
    {
        return new Genre
        {
            Name = name
        };
    }
}
