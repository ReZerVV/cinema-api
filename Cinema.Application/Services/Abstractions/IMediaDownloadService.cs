using Cinema.Domain.Movies.Entities;

namespace Cinema.Application.Services.Abstractions;

internal interface IMediaDownloadService
{
    bool IsDownloading();
    void Download(Media media);
}
