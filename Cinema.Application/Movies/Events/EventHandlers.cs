using Cinema.Application.Movies.Services.Abstractions;
using Cinema.Domain.Common;
using Cinema.Domain.Movies.Entities;
using Cinema.Domain.Movies.Enums;
using Cinema.Domain.Movies.Events;
using MediatR;

namespace Cinema.Application.Movies.Events;

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

    public Task Handle(DownloadAddedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        if (_mediaDownloadService.IsDownloading())
        { 
            media.Status = DownloadStatus.Waiting;
        }
        else
        {
            media.Status = DownloadStatus.Downloading;
            _mediaDownloadService.Download(media);
        }
        _unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }

    public Task Handle(DownloadStartedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = DownloadStatus.Downloading;
        _unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }

    public Task Handle(DownloadCompletedEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = DownloadStatus.Downloaded;
        media.FileName = notification.fileName;
        _unitOfWork.SaveChanges();
        if (_unitOfWork.Medias.GetWaitingDownloads().FirstOrDefault() is Media nextMedia)
            _mediaDownloadService.Download(nextMedia);
        return Task.CompletedTask;
    }

    public Task Handle(DownloadCanceledEvent notification, CancellationToken cancellationToken)
    {
        var media = _unitOfWork.Medias.GetById(notification.Id);
        media.Status = DownloadStatus.Error;
        _unitOfWork.SaveChanges();
        return Task.CompletedTask;
    }
}