using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Dtos;

public class LoadingDto
{
    public MovieDto Movie { get; set; }
    public string Status { get; set; }

    public LoadingDto(Movie movie, string baseUrl)
    {
        Movie = new MovieDto(movie, baseUrl);
        Status = GetStatus(movie.Medias);
    }

    private string GetStatus(IEnumerable<Media> medias)
    {
        if (medias.Any(m => m.Status == Domain.Movies.Enums.LoadingStatus.Error))
            return Domain.Movies.Enums.LoadingStatus.Error.ToString();
        if (medias.Any(
                m => m.Status == Domain.Movies.Enums.LoadingStatus.Downloading ||
                m.Status == Domain.Movies.Enums.LoadingStatus.Downloaded)
            )
            return Domain.Movies.Enums.LoadingStatus.Downloading.ToString();
        else
            return Domain.Movies.Enums.LoadingStatus.Waiting.ToString();
    }
}
