using Cinema.Domain.Common;

namespace Cinema.Domain.Movies.Events;

public record DownloadStartedEvent(string Id) : IDomainEvent;