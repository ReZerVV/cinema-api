namespace Cinema.Application.Utils.Api.Kinopoisk.Models;

public class FilmInfo
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string AlternativeName { get; init; }
    public string Description { get; init; }
    public string ShortDescription { get; init; }
    public string Type { get; init; }
    public int Year { get; init; }
    public int MovieLength { get; init; }
    public ImdbInfo Rating { get; init; }
    public ImdbInfo Votes { get; init; }
    public UrlInfo Poster { get; init; }
    public UrlInfo Backdrop { get; init; }
    public IEnumerable<NameInfo> Genres { get; init; }
    public IEnumerable<NameInfo> Countries { get; init; }
}
