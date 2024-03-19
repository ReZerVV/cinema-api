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

    public IEnumerable<Movie> Search(string? query = null, string? sort = null, string? type = null, string? genre = null)
    {
        var movies = _db.Movies
            .Include(m => m.Medias)
            .Include(m => m.Genres)
            .Where(m => !m.Medias.Any(m => m.Status != Domain.Movies.Enums.LoadingStatus.Downloaded))
            .Where(m =>
                (query == null || (
                        m.Name.ToUpper().Contains(query.ToUpper()) ||
                        m.EnName.ToUpper().Contains(query.ToUpper()) ||
                        m.Description.ToUpper().Contains(query.ToUpper()) ||
                        m.ShortDescription.ToUpper().Contains(query.ToUpper())
                    )
                ) &&
                (type == null || m.Type.ToUpper() == type.ToUpper()) &&
                (genre == null || m.Type.ToUpper() == type.ToUpper())
            );
        return sort switch
        {
            "popular" => movies.OrderByDescending(m => m.Votes),
            "rating" => movies.OrderByDescending(m => m.Rating),
            _ => movies
        };
    }

    public void Update(Movie entity)
    {
    }
}
