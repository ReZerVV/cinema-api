using Cinema.Domain.Common;
using Cinema.Domain.Movies.Enums;
using Cinema.Domain.Movies.Events;

namespace Cinema.Domain.Movies.Entities;

public class Media : Aggregate
{
    public string? FileName { get; set; }
    public string? Url { get; set; }
    public LoadingStatus Status { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public MediaType Type { get; set; }
    public string MovieId { get; set; }
    public Movie? Movie { get; set; }

    public Media() : base(Guid.NewGuid().ToString())
    {
        
    }

    public static Media CreateDownload(string url, MediaType type)
    {
        var media = new Media
        {
            Url = url,
            Status = LoadingStatus.Waiting,
            Type = type,
        };
        media.RaiseEvent(new DownloadAddedEvent(media.Id));
        return media;
    }

    public static Media Create(string fileName, MediaType type)
    {
        var media = new Media
        {
            FileName = fileName,
            Status = LoadingStatus.Downloaded,
            Type = type,
        };
        return media;
    }
}