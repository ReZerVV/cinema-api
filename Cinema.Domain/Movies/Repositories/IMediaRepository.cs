using Cinema.Domain.Common;
using Cinema.Domain.Movies.Entities;

namespace Cinema.Domain.Movies.Repositories;

public interface IMediaRepository : IRepository<Media>
{
    IEnumerable<Media> GetWaitingDownloads();
}
