using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Enums;

namespace Cinema.Domain.Movies.Entities;

public class Movie : AggregateRoot
{
    public string Name { get; set; }
    public string EnName { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public string Type { get; set; }
    public int Year { get; set; }
    public int MovieLength { get; set; }
    public string Country { get; set; }
    public float Rating { get; set; }
    public int Votes { get; set; }
    public ICollection<Media> Medias { get; set; } = new List<Media>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();


    public Movie() : base(Guid.NewGuid().ToString())
    {

    }

    public static Movie Create(
        string name, string enName, string description, string shortDescription,
        string type, int year, int movieLength, string country, float rating, int votes, IEnumerable<Genre> genres)
    {
        var movie = new Movie()
        {
            Name = name,
            EnName = enName,
            Description = description,
            ShortDescription = shortDescription,
            Type = type,
            Year = year,
            MovieLength = movieLength,
            Country = country,
            Rating = rating,
            Votes = votes,
            Genres = genres.ToList(),
        };
        return movie;
    }

    public void AddMedia(Media media)
    {
        media.MovieId = Id;
        media.Movie = this;
        Medias.Add(media);
    }
}
