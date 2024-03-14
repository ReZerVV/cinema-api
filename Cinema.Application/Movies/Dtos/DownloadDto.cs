using Cinema.Application.Genres.Dtos;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Movies.Dtos;

public class DownloadDto
{
    public string Id { get; set; }
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
    public string Status { get; set; }
    public ICollection<GenreDto> Genres { get; set; }

    public DownloadDto(Movie movie)
    {
        Id = movie.Id;
        Name = movie.Name;
        EnName = movie.EnName;
        Description = movie.Description;
        ShortDescription = movie.ShortDescription;
        Type = movie.Type;
        Year = movie.Year;
        MovieLength = movie.MovieLength;
        Country = movie.Country;
        Rating = movie.Rating;
        Votes = movie.Votes;
        Status = GetStatus(movie.Medias);
        Genres = movie.Genres.Select(g => new GenreDto(g)).ToList();
    }

    private string GetStatus(IEnumerable<Media> medias)
    {
        if (medias.Any(m => m.Status == Domain.Movies.Enums.DownloadStatus.Error))
            return Domain.Movies.Enums.DownloadStatus.Error.ToString();
        if (medias.Any(
                m => m.Status == Domain.Movies.Enums.DownloadStatus.Downloading ||
                m.Status == Domain.Movies.Enums.DownloadStatus.Downloaded)
            )
            return Domain.Movies.Enums.DownloadStatus.Downloading.ToString();
        else
            return Domain.Movies.Enums.DownloadStatus.Waiting.ToString();
    }
}
