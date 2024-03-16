using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Services.Abstractions;

internal interface IMediaDownloadService
{
    bool IsDownloading();
    Task Download(Media media);
}
