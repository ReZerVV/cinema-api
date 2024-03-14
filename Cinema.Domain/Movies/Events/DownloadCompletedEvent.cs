using Cinema.Domain.Common;

namespace Cinema.Domain.Movies.Events;

public record DownloadCompletedEvent(string Id, string fileName) : IDomainEvent;