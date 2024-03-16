using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Dtos;

public class MovieDto
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
    public string? Poster { get; set; }
    public string? Backdrop { get; set; }
    public string? Video { get; set; }
    public ICollection<GenreDto> Genres { get; set; }

    public MovieDto(Movie movie, string baseUrl)
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
        var posterFileName = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Poster).FileName;
        Poster = posterFileName != null ? $"{baseUrl}/{posterFileName}" : null;
        var backdropFileName = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Backdrop).FileName;
        Backdrop = backdropFileName != null ? $"{baseUrl}/{backdropFileName}" : null;
        var videoFileName = movie.Medias.First(m => m.Type == Domain.Movies.Enums.MediaType.Video).FileName;
        Video = videoFileName != null ? $"{baseUrl}/{videoFileName}" : null;
        Genres = movie.Genres.Select(g => new GenreDto(g)).ToList();
    }
}