using Cinema.Domain.Common;

namespace Cinema.Domain.Movies.Events;

public record DownloadCanceledEvent(string Id) : IDomainEvent;