using Cinema.Domain.Genres.Entities;
using Cinema.Domain.Genres.Repositories;

namespace Cinema.Infrastructure.Persistence.Repositories;

internal class GenreRepository : IGenreRepository
{
    private readonly CinemaDbContext _db;

    public GenreRepository(CinemaDbContext db)
    {
        _db = db;
    }

    public void Create(Genre entity)
    {
        _db.Add(entity);
    }

    public void Delete(Genre entity)
    {
        _db.Remove(entity);
    }

    public IEnumerable<Genre> GetAll()
    {
        return _db.Genres;
    }

    public Genre? GetById(string id)
    {
        return _db.Genres.SingleOrDefault(g => g.Id == id);
    }

    public Genre? GetByName(string name)
    {
        return _db.Genres.SingleOrDefault(g => g.Name == name);
    }

    public void Update(Genre entity)
    {
    }
}
