using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Enums;

namespace Cinema.Domain.Movies.Entities;

public class Movie : AggregateRoot
{
    public int? KinopoiskId { get; set; }
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
        string type, int year, int movieLength, string country, float rating, int votes,
        string posterDownloadUrl, string backdropDownloadUrl, string videoDownloadUrl,
        IEnumerable<Genre> genres, int? kinopoiskId = null)
    {
        var movie = new Movie()
        {
            KinopoiskId = kinopoiskId,
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
        movie.Medias = new List<Media>
        {
            Media.Create(movie, posterDownloadUrl, MediaType.Poster),
            Media.Create(movie, backdropDownloadUrl, MediaType.Backdrop),
            Media.Create(movie, videoDownloadUrl, MediaType.Video),
        };
        return movie;
    }
}
