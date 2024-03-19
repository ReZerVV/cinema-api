using Cinema.Domain.Movies.Entities;
using Cinema.Domain.Movies.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Infrastructure.Persistence.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly CinemaDbContext _db;

    public MovieRepository(CinemaDbContext db)
    {
        _db = db;
    }

    public void Create(Movie entity)
    {
        _db.Add(entity);
    }

    public void Delete(Movie entity)
    {
        _db.Remove(entity);
    }

    public IEnumerable<Movie> GetAll()
    {
        return _db.Movies
            .Include(m => m.Medias)
            .Include(m => m.Genres);
    }

    public Movie? GetById(string id)
    {
        return _db.Movies
            .Include(m => m.Medias)
            .Include(m => m.Genres)
            .SingleOrDefault(m => m.Id == id);
    }

    public IEnumerable<Movie> GetDownloads()
    {
        return _db.Movies
            .Include(m => m.Medias)
            .Include(m => m.Genres)
            .Where(m => !m.Medias.Any(m => m.Status != Domain.Movies.Enums.LoadingStatus.Downloaded));
    }

    public void Update(Movie entity)
    {
    }
}
