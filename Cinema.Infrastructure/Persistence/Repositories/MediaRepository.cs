using Cinema.Domain.Movies.Entities;
using Cinema.Domain.Movies.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence.Repositories;

internal class MediaRepository : IMediaRepository
{
    private readonly CinemaDbContext _db;

    public MediaRepository(CinemaDbContext db)
    {
        _db = db;
    }

    public void Create(Media entity)
    {
        _db.Add(entity);
    }

    public void Delete(Media entity)
    {
        _db.Remove(entity);
    }

    public IEnumerable<Media> GetAll()
    {
        return _db.Medias;
    }

    public Media? GetById(string id)
    {
        return _db.Medias
            .Include(m => m.Movie)
            .SingleOrDefault(m => m.Id == id);
    }

    public IEnumerable<Media> GetWaitingDownloads()
    {
        return _db.Medias
            .Include(m => m.Movie)
            .Where(m => m.Status == Domain.Movies.Enums.DownloadStatus.Waiting)
            .OrderBy(m => m.CreateAt);
    }

    public void Update(Media entity)
    {
    }
}
