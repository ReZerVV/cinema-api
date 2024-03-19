using Cinema.Application.Services.Abstractions;
using Cinema.Domain.Movies.Entities;
using Cinema.Domain.Movies.Events;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Cinema.Application.Services;

internal class MediaDownloadService : IMediaDownloadService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHostingEnvironment _hostingEnvironment;
    public bool isDownloading;

    public MediaDownloadService(
        IServiceScopeFactory serviceScopeFactory,
        IHostingEnvironment hostingEnvironment)
    {
        isDownloading = false;
        _serviceScopeFactory = serviceScopeFactory;
        _hostingEnvironment = hostingEnvironment;
    }

    public void Download(Media media)
    {
        try
        {
            isDownloading = true;
            using WebClient webClient = new WebClient();
            webClient.OpenRead(new Uri(media.Url));
            var contentType = webClient.ResponseHeaders?["Content-Type"];
            if (contentType == null || contentType != "video/mp4" && contentType != "image/png" && contentType != "image/jpeg")
            {
                PublishEvent(new DownloadCanceledEvent(media.Id));
                isDownloading = false;
                return;
            }
            string fileName = GenerateFileNameByContentType(contentType);
            webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(
                (sender, e) =>
                {
                    if (e.Error != null)
                        PublishEvent(new DownloadCanceledEvent(media.Id));
                    else
                        PublishEvent(new DownloadCompletedEvent(media.Id, fileName));
                    isDownloading = false;
                }
            );
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                (sender, e) =>
                {
                    Console.WriteLine($"{e.ProgressPercentage}({e.BytesReceived}/{e.TotalBytesToReceive})");
                }
            );
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, fileName);
            webClient.DownloadFileAsync(new Uri(media.Url), filePath);
        }
        catch (Exception e)
        {
            PublishEvent(new DownloadCanceledEvent(media.Id));
            isDownloading = false;
        }
    }

    public bool IsDownloading()
    {
        return isDownloading;
    }

    private void PublishEvent(INotification @event)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            mediator.Publish(@event);
        }
    }

    private string GenerateFileNameByContentType(string contentType)
    {
        var contentTypeParts = contentType.Split('/');
        string type = contentTypeParts[0];
        string extension = contentTypeParts[1];
        return Path.Combine(type[0].ToString(), $"{Guid.NewGuid()}.{extension}");
    }
}
