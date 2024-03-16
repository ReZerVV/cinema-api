using Cinema.Application.Services.Abstractions;
using Cinema.Application.Utils.Api.Kinogo;
using Cinema.Application.Utils.Api.Kinopoisk;
using Cinema.Domain.Common;
using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Movies.Entities;
using Cinema.Domain.Movies.Enums;
using Cinema.Domain.Movies.Events;
using MediatR;

namespace Cinema.Application.Events;

internal class EventHandlers :
    INotificationHandler<DownloadAddedEvent>,
    INotificationHandler<DownloadStartedEvent>,
    INotificationHandler<DownloadCompletedEvent>,
    INotificationHandler<DownloadCanceledEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaDownloadService _mediaDownloadService;

    public EventHandlers(
        IUnitOfWork unitOfWork,
        IMediaDownloadService mediaDownloadService)
    {
        _unitOfWork = unitOfWork;
        _mediaDownloadService = mediaDownloadService;
    }

    public async Task Handle(DownloadAddedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        if (_mediaDownloadService.IsDownloading())
        {
            media.Status = LoadingStatus.Waiting;
        }
        else
        {
            media.Status = LoadingStatus.Downloading;
            await _mediaDownloadService.Download(media);
        }
        _unitOfWork.SaveChanges();
    }

    public Task Handle(DownloadStartedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = LoadingStatus.Downloading;
        _unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }

    public Task Handle(DownloadCompletedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = LoadingStatus.Downloaded;
        media.FileName = notification.fileName;
        _unitOfWork.SaveChanges();
        if (_unitOfWork.Medias.GetWaitingDownloads().FirstOrDefault() is Media nextMedia)
            _mediaDownloadService.Download(nextMedia);
        return Task.CompletedTask;
    }

    public Task Handle(DownloadCanceledEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = LoadingStatus.Error;
        _unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }
}