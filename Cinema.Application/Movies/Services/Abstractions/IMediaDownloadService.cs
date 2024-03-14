using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Movies.Services.Abstractions;

internal interface IMediaDownloadService
{
    bool IsDownloading();
    void Download(Media media);
}
