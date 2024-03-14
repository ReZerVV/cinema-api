using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Movies.Dtos;

public class MediaDto
{
    public string Id { get; set; }
    public string? Url { get; set; }
    public string DownloadUrl { get; set; }
    public string Status { get; set; }

    public MediaDto(Media media, string baseUrl)
    {
        Id = media.Id;
        Url = media.FileName != null ? $"{baseUrl}/{media.FileName}" : null;
        DownloadUrl = media.Url;
        Status = media.Status.ToString();
    }
}
