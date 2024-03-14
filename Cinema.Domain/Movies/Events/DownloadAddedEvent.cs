using Cinema.Domain.Common;

namespace Cinema.Domain.Movies.Events;

public record DownloadAddedEvent(string Id) : IDomainEvent;